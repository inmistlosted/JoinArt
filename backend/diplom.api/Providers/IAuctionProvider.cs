using diplom.api.Models;
using diplom.api.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Providers
{
    public interface IAuctionProvider
    {
        Task<bool> ExistsBid(int paintingId);
        Task<GetBidResponseModel> CreateBid(int paintingId, double price);
        Task<int> GetBidIdByPainting(int paintingId);
        Task<Bid> GetBid(int bidId);
        Task UpdateBidStatus(int bidId, bool status);
        Task<IList<BidHistoryItem>> GetBidHistory(int bidId);
        Task StartBid(int bidId, int userId, double price);
        Task PlaceBet(int bidId, int userId, double price);
        Task<IList<Bid>> GetTopBids(int take, bool withCache = false);
        Task<IList<Bid>> GetAllBids(bool withCache = false);
    }
}
