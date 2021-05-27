using diplom.api.Classes;
using diplom.api.DataAccessLayer;
using diplom.api.Models;
using diplom.api.Models.RequestModels;
using diplom.api.Models.ResponseModels;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Providers.Implementation
{
    public class AuctionProvider : IAuctionProvider
    {
        private readonly IDataAccessAdapter _dataAccessAdapter;
        private readonly IMemoryCache _cache;

        public AuctionProvider(IDataAccessAdapter dataAccessAdapter, IMemoryCache cache)
        {
            this._dataAccessAdapter = dataAccessAdapter ?? throw new ArgumentNullException(nameof(dataAccessAdapter));
            this._cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<GetBidResponseModel> CreateBid(int paintingId, double price)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price));
            }

            bool isAdded = false;
            int bidId = 0;

            using (IDataReader reader = await this._dataAccessAdapter.AuctionAdapter.Create(paintingId, price, true, DateTime.MinValue))
            {
                if (reader.Read())
                {
                    bidId = (int)reader["bidId"];
                    isAdded = bidId != 0;
                }
            }

            GetBidResponseModel response = new GetBidResponseModel
            {
                Status = isAdded,
                BidId = bidId,
                Message = isAdded ? string.Empty : "Unexpected error occurred",
            };

            return response;
        }

        public async Task<int> GetBidIdByPainting(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            int bidId = 0;

            using (IDataReader reader = await this._dataAccessAdapter.AuctionAdapter.GetBidIdByPainting(paintingId))
            {
                if (reader.Read())
                {
                    bidId = (int)reader["bidId"];
                }
            }

            return bidId;
        }

        public async Task<bool> ExistsBid(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            int bidId = await GetBidIdByPainting(paintingId);

            return bidId != 0;
        }

        public async Task<Bid> GetBid(int bidId)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            Bid bid = null;

            using (IDataReader reader = await this._dataAccessAdapter.AuctionAdapter.Get(bidId))
            {
                if (reader.Read())
                {
                    bid = ConvertReaderToBid(reader);
                }
            }

            return bid;
        }

        public async Task UpdateBidStatus(int bidId, bool status)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            await this._dataAccessAdapter.AuctionAdapter.UpdateBidStatus(bidId, status);
        }

        public async Task<IList<BidHistoryItem>> GetBidHistory(int bidId)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            IList<BidHistoryItem> history = new List<BidHistoryItem>();

            using (IDataReader reader = await this._dataAccessAdapter.AuctionAdapter.GetBidHistory(bidId))
            {
                while (reader.Read())
                {
                    history.Add(new BidHistoryItem {
                        BetId = (int)reader["betId"],
                        UserLogin = reader["login"].ToString(),
                        Bet = (double)reader["bet"],
                        PlaceBetTime = Convert.ToDateTime(reader["placeTime"]),
                    });
                }
            }

            return history.OrderByDescending(x => x.PlaceBetTime).ToList();
        }

        public async Task StartBid(int bidId, int userId, double price)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            DateTime bidStartTime = DateTime.Now;

            await this._dataAccessAdapter.AuctionAdapter.UpdateBidStartTime(bidId, bidStartTime);
            await this._dataAccessAdapter.AuctionAdapter.UpdateBidBuyer(bidId, userId);
            await this._dataAccessAdapter.AuctionAdapter.AddBidHistoryItem(bidId, userId, price, bidStartTime);
        }

        public async Task PlaceBet(int bidId, int userId, double price)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            await this._dataAccessAdapter.AuctionAdapter.UpdateBidBuyer(bidId, userId);
            await this._dataAccessAdapter.AuctionAdapter.UpdateBidPrice(bidId, price);
            await this._dataAccessAdapter.AuctionAdapter.AddBidHistoryItem(bidId, userId, price, DateTime.Now);
        }

        public async Task<IList<Bid>> GetAllBids(bool withCache = false)
        {
            string cacheKey = $"get_bids";

            IList<Bid> bids = withCache ? Helper.GetFromCache<IList<Bid>>(this._cache, cacheKey) : null;

            if (bids == null)
            {
                bids = new List<Bid>();

                using (IDataReader reader = await this._dataAccessAdapter.AuctionAdapter.GetAll())
                {
                    while (reader.Read())
                    {
                        bids.Add(ConvertReaderToBid(reader));
                    }
                }

                foreach(Bid bid in bids)
                {
                    bid.PaintingImage = await GetAuctionImage(bid.BidId);
                }

                Helper.SetToCache(this._cache, cacheKey, bids);
            }

            return bids;
        }

        public async Task<IList<Bid>> GetTopBids(int take, bool withCache = false)
        {
            if (take <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(take));
            }

            string cacheKey = "get_top_bids";

            IList<Bid> topBids = withCache ? Helper.GetFromCache<IList<Bid>>(this._cache, cacheKey) : null;

            if (topBids == null)
            {
                IList<Bid> allBids = await GetAllBids();

                if (allBids != null)
                {
                    IDictionary<int, long> likes = await GetBidsLikes(allBids);

                    foreach (Bid bid in allBids)
                    {
                        bid.LikesCount = likes.ContainsKey(bid.BidId) ? likes[bid.BidId] : 0;
                    }

                    topBids = allBids.OrderByDescending(x => x.LikesCount).Take(take).ToList();

                    Helper.SetToCache(this._cache, cacheKey, topBids);
                }
            }

            return topBids;
        }

        private async Task<IDictionary<int, long>> GetBidsLikes(IList<Bid> bids)
        {
            IDictionary<int, long> bidsLikes = new Dictionary<int, long>();

            foreach (Bid bid in bids)
            {
                using (IDataReader reader = await this._dataAccessAdapter.AlbumAdapter.GetAlbumLikesCount(bid.BidId))
                {
                    if (reader.Read())
                    {
                        long likesCount = (long)reader["count"];

                        bidsLikes.Add(bid.BidId, likesCount);
                    }
                }
            }

            return bidsLikes;
        }

        private async Task<string> GetAuctionImage(int auctionId)
        {
            using (IDataReader reader = await this._dataAccessAdapter.AuctionAdapter.GetAuctionImage(auctionId))
            {
                if (reader.Read())
                {
                    return Helper.ToRenderablePictureString(reader["Image"] == DBNull.Value ? null : (byte[])reader["Image"]);
                }
            }

            return string.Empty;
        }

        private Bid ConvertReaderToBid(IDataReader reader)
        {
            return new Bid
            {
                BidId = (int)reader["bidId"],
                CurrentPrice = (double)reader["bidprice"],
                Status = (bool)reader["status"],
                PaintingId = (int)reader["paintingId"],
                StartTime = Convert.ToDateTime(reader["startTime"]),
            };
        }
    }
}
