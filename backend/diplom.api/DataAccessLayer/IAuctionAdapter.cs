using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer
{
    public interface IAuctionAdapter
    {
        Task<IDataReader> Create(int paintingId, double price, bool status, DateTime startTime);
        Task<IDataReader> GetBidIdByPainting(int paintingId);
        Task<IDataReader> Get(int bidId);
        Task UpdateBidStatus(int bidId, bool status);
        Task<IDataReader> GetBidHistory(int bidId);
        Task UpdateBidStartTime(int bidId, DateTime startTime);
        Task UpdateBidBuyer(int bidId, int userId);
        Task UpdateBidPrice(int bidId, double price);
        Task AddBidHistoryItem(int bidId, int userId, double bet, DateTime placeTime);
        Task<IDataReader> GetAll();
        Task<IDataReader> GetBidLikesCount(int bidId);
        Task<IDataReader> GetAuctionImage(int auctionId);
    }
}
