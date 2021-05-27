using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer.Implementation
{
    public class OrderAdapter : IOrderAdapter
    {
        private ICommandAdapter _commandAdapter;

        public OrderAdapter(ICommandAdapter commandAdapter)
        {
            this._commandAdapter = commandAdapter ?? throw new ArgumentNullException(nameof(commandAdapter));
        }

        public async Task<IDataReader> CreateOrder(int userId, DateTime date, double amount, bool status)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            using (NpgsqlCommand sqlCommand = CreateCreateOrderCommand(userId, date, amount, status))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task AddOrderItem(int orderId, int paintingId, int count)
        {
            if (orderId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderId));
            }

            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            using (NpgsqlCommand sqlCommand = CreateAddOrderItemCommand(orderId, paintingId, count))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetUserOrders(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetUserOrdersCommand(userId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetOrderItems(int orderId)
        {
            if (orderId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetOrderItemsCommand(orderId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task PayOrder(int orderId)
        {
            if (orderId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderId));
            }

            using (NpgsqlCommand sqlCommand = CreatePayOrderCommand(orderId))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        private NpgsqlCommand CreateCreateOrderCommand(int userId, DateTime date, double amount, bool status)
        {
            string query = @"insert into orders (userId, date, amount, status)
                             values (@userId, @date, @amount, @status)
                             returning orderId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("userId", userId);
            sqlCommand.Parameters.AddWithValue("date", date);
            sqlCommand.Parameters.AddWithValue("amount", amount);
            sqlCommand.Parameters.AddWithValue("status", status);

            return sqlCommand;
        }

        private NpgsqlCommand CreateAddOrderItemCommand(int orderId, int paintingId, int count)
        {
            string query = @"insert into orderpaintings (orderId, paintingId, count)
                             values (@orderId, @paintingId, @count)";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("orderId", orderId);
            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);
            sqlCommand.Parameters.AddWithValue("count", count);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetUserOrdersCommand(int userId)
        {
            string query = @"select * from orders where userId = @userId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("userId", userId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetOrderItemsCommand(int orderId)
        {
            string query = @"select * from orderpaintings where orderId = @orderId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("orderId", orderId);

            return sqlCommand;
        }

        private NpgsqlCommand CreatePayOrderCommand(int orderId)
        {
            string query = @"update orders set status = true where orderId = @orderId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("orderId", orderId);

            return sqlCommand;
        }
    }
}
