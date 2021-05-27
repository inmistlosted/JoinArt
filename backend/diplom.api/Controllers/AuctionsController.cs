using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diplom.api.DataAccessLayer;
using diplom.api.Models;
using diplom.api.Models.ResponseModels;
using diplom.api.Providers;
using diplom.api.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace diplom.api.Controllers
{
    [Route("auctions")]
    public class AuctionsController : BaseController
    {
        const int TopBidsTake = 6;

        private readonly IAuctionProvider _auctionProvider;
        private readonly IPaintingProvider _paintingProvider;

        public AuctionsController(IDataAccessAdapter dataAccessAdapter, IMemoryCache cache, IOptions<AppSettings> settings, IAuctionProvider auctionProvider, IPaintingProvider paintingProvider)
            : base(dataAccessAdapter, cache, settings)
        {
            this._auctionProvider = auctionProvider ?? throw new ArgumentNullException(nameof(auctionProvider));
            this._paintingProvider = paintingProvider ?? throw new ArgumentNullException(nameof(paintingProvider));
        }

        [HttpPost, Route("get-painting-bid/{paintingId}")]
        public async Task<IActionResult> GetPaintingBid(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            GetBidResponseModel response;

            if (await _auctionProvider.ExistsBid(paintingId))
            {
                response = new GetBidResponseModel
                {
                    BidId = await _auctionProvider.GetBidIdByPainting(paintingId),
                    Status = true,
                };
            }
            else
            {
                double price = await _paintingProvider.GetPaintingStartPrice(paintingId);

                response = await _auctionProvider.CreateBid(paintingId, price); 
            }

            return Json(response);
        }

        [HttpGet, Route("get-bid/{bidId}")]
        public async Task<IActionResult> GetBid(int bidId)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            Bid bid = await _auctionProvider.GetBid(bidId);

            if(bid == null)
            {
                return BadRequest();
            }

            bid.HasStarted = bid.StartTime != DateTime.MinValue;

            if (bid.HasStarted)
            {
                bid.EndTime = bid.StartTime.AddDays(Settings.AuctionDurationInDays);

                if (bid.Status && DateTime.Now > bid.EndTime)
                {
                    await _auctionProvider.UpdateBidStatus(bidId, false);
                    bid.Status = false;
                }
            }

            PaintingResponseModel painting = await _paintingProvider.GetPainting(bid.PaintingId, 0);

            if(painting == null)
            {
                return BadRequest();
            }

            bid.PaintingTitle = painting.Title;
            bid.PaintingImage = painting.ImagePath;

            bid.History = await _auctionProvider.GetBidHistory(bidId);

            return Json(bid);
        }

        [HttpPost, Route("start-bid/{bidId}/{userId}")]
        public async Task<IActionResult> PlaceBet(int bidId, int userId)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            Bid bid = await _auctionProvider.GetBid(bidId);

            if (bid == null)
            {
                return BadRequest();
            }

            bid.HasStarted = bid.StartTime != DateTime.MinValue;
            GetBidResponseModel response = null;

            if (bid.HasStarted)
            {
                response = new GetBidResponseModel
                {
                    Status = false,
                    Message = "Auction has already started, please reload page",
                };
            }
            else
            {
                await _auctionProvider.StartBid(bidId, userId, bid.CurrentPrice);

                response = new GetBidResponseModel
                {
                    Status = true,
                };
            }

            return Json(response);
        }

        [HttpPost, Route("place-bet/{bidId}/{userId}/{price}")]
        public async Task<IActionResult> PlaceBet(int bidId, int userId, double price)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price));
            }

            Bid bid = await _auctionProvider.GetBid(bidId);

            if (bid == null || price <= bid.CurrentPrice)
            {
                return BadRequest();
            }

            GetBidResponseModel response = null;
            bid.HasStarted = bid.StartTime != DateTime.MinValue;

            if (!bid.HasStarted)
            {
                await _auctionProvider.StartBid(bidId, userId, bid.CurrentPrice);

                response = new GetBidResponseModel
                {
                    Status = true,
                };

                return Json(response);
            }

            bid.EndTime = bid.StartTime.AddDays(Settings.AuctionDurationInDays);

            if (!bid.Status || DateTime.Now > bid.EndTime)
            {
                await _auctionProvider.UpdateBidStatus(bidId, false);

                response = new GetBidResponseModel
                {
                    Status = false,
                    Message = "Auction has already ended, please reload page"
                };

                return Json(response);
            }

            await _auctionProvider.PlaceBet(bidId, userId, price);
            await _paintingProvider.UpdatePaintingPrice(bid.PaintingId, price);

            response = new GetBidResponseModel
            {
                Status = true,
            };

            return Json(response);
        }

        [HttpGet, Route("get-top-bids")]
        public async Task<IActionResult> GetTopBids()
        {
            IList<Bid> topBids = await _auctionProvider.GetTopBids(TopBidsTake);

            return Json(topBids);
        }

        [HttpGet, Route("get-all-bids")]
        public async Task<IActionResult> GetAllBids()
        {
            IList<Bid> bids = await _auctionProvider.GetAllBids();

            return Json(bids);
        }
    }
}
