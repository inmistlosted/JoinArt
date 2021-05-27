using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer.Implementation
{
    public class AuctionAdapter : IAuctionAdapter
    {
        private ICommandAdapter _commandAdapter;

        public AuctionAdapter(ICommandAdapter commandAdapter)
        {
            this._commandAdapter = commandAdapter ?? throw new ArgumentNullException(nameof(commandAdapter));
        }

        public async Task<IDataReader> Create(int paintingId, double price, bool status, DateTime startTime)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price));
            }

            using (NpgsqlCommand sqlCommand = CreateCreateCommand(paintingId, price, status, startTime))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetBidIdByPainting(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetBidIdByPaintingCommand(paintingId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> Get(int bidId)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetCommand(bidId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task UpdateBidStatus(int bidId, bool status)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            using (NpgsqlCommand sqlCommand =CreateUpdateBidStatusCommand(bidId, status))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetBidHistory(int bidId)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetBidHistoryCommand(bidId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task UpdateBidStartTime(int bidId, DateTime startTime)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            using (NpgsqlCommand sqlCommand = CreateUpdateBidStartTimeCommand(bidId, startTime))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task UpdateBidBuyer(int bidId, int userId)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            using (NpgsqlCommand sqlCommand = CreateUpdateBidBuyerCommand(bidId, userId))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task UpdateBidPrice(int bidId, double price)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price));
            }

            using (NpgsqlCommand sqlCommand = CreateUpdateBidPriceCommand(bidId, price))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task AddBidHistoryItem(int bidId, int userId, double bet, DateTime placeTime)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            if (userId <+ 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            if (bet <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bet));
            }

            using (NpgsqlCommand sqlCommand = CreateAddBidHistoryItemCommand(bidId, userId, bet, placeTime))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetAll()
        {
            using (NpgsqlCommand sqlCommand = CreateGetAllCommand())
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetBidLikesCount(int bidId)
        {
            if (bidId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bidId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetBidLikesCountCommand(bidId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetAuctionImage(int auctionId)
        {
            if (auctionId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(auctionId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetAuctionImageCommand(auctionId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        private NpgsqlCommand CreateCreateCommand(int paintingId, double price, bool status, DateTime startTime)
        {
            string query = @"insert into bidproducts (bidprice, status, paintingId, startTime)
                             values (@bidprice, @status, @paintingId, @startTime)
                             returning bidId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("bidprice", price);
            sqlCommand.Parameters.AddWithValue("status", status);
            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);
            sqlCommand.Parameters.AddWithValue("startTime", startTime);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetBidIdByPaintingCommand(int paintingId)
        {
            string query = @"select bidId from bidproducts where paintingId = @paintingId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetCommand(int bidId)
        {
            string query = @"select * from bidproducts where bidId = @bidId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("bidId", bidId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateUpdateBidStatusCommand(int bidId, bool status)
        {
            string query = @"update bidproducts set status = @status where bidId = @bidId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("bidId", bidId);
            sqlCommand.Parameters.AddWithValue("status", status);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetBidHistoryCommand(int bidId)
        {
            string query = @"select betId, users.login, bet, placetime 
                             from bidbets inner join users on bidbets.userid = users.userid 
                             where bidId = @bidId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("bidId", bidId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateUpdateBidStartTimeCommand(int bidId, DateTime startTime)
        {
            string query = @"update bidproducts set startTime = @startTime where bidId = @bidId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("bidId", bidId);
            sqlCommand.Parameters.AddWithValue("startTime", startTime);

            return sqlCommand;
        }

        private NpgsqlCommand CreateUpdateBidBuyerCommand(int bidId, int userId)
        {
            string query = @"update bidproducts set userId = @userId where bidId = @bidId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("bidId", bidId);
            sqlCommand.Parameters.AddWithValue("userId", userId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateUpdateBidPriceCommand(int bidId, double price)
        {
            string query = @"update bidproducts set bidprice = @price where bidId = @bidId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("bidId", bidId);
            sqlCommand.Parameters.AddWithValue("price", price);

            return sqlCommand;
        }

        private NpgsqlCommand CreateAddBidHistoryItemCommand(int bidId, int userId, double bet, DateTime placeTime)
        {
            string query = @"insert into bidbets (bidId, userId, bet, placeTime)
                             values (@bidId, @userId, @bet, @placeTime);";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("bidId", bidId);
            sqlCommand.Parameters.AddWithValue("userId", userId);
            sqlCommand.Parameters.AddWithValue("bet", bet);
            sqlCommand.Parameters.AddWithValue("placeTime", placeTime);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetAllCommand()
        {
            string query = @"select * from bidproducts";

            var sqlCommand = new NpgsqlCommand(query);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetBidLikesCountCommand(int bidId)
        {
            string query = @"select count(likeId)
                             from likes
                             where paintingid in (
                                    select paintingId 
                                    from bidproducts 
                                    where bidId = @bidId
                             )";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("bidId", bidId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetAuctionImageCommand(int bidId)
        {
            string query = @"select image 
                             from paintings 
                             where paintingid in (
                                    select paintingId 
                                    from bidproducts 
                                    where bidId = @bidId
                             ) limit 1";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("bidId", bidId);

            return sqlCommand;
        }
    }
}
