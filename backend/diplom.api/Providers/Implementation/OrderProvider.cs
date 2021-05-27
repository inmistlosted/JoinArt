using diplom.api.DataAccessLayer;
using diplom.api.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Providers.Implementation
{
    public class OrderProvider : IOrderProvider
    {
        private readonly IDataAccessAdapter _dataAccessAdapter;
        private readonly IMemoryCache _cache;

        public OrderProvider(IDataAccessAdapter dataAccessAdapter, IMemoryCache cache)
        {
            this._dataAccessAdapter = dataAccessAdapter ?? throw new ArgumentNullException(nameof(dataAccessAdapter));
            this._cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<int> CreateOrder(int userId, double amount)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            int orderId = 0;

            using (IDataReader reader = await this._dataAccessAdapter.OrderAdapter.CreateOrder(userId, DateTime.Now, amount, false))
            {
                if (reader.Read())
                {
                    orderId = (int)reader["orderId"];
                }
            }

            return orderId;
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

            await this._dataAccessAdapter.OrderAdapter.AddOrderItem(orderId, paintingId, count);
        }

        public async Task<IList<Order>> GetUserOrders(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            IList<Order> orders = new List<Order>();

            using (IDataReader reader = await this._dataAccessAdapter.OrderAdapter.GetUserOrders(userId))
            {
                while (reader.Read())
                {
                    orders.Add(ConvertReaderToOrder(reader));
                }
            }

            return orders;
        }

        public async Task<IDictionary<int, int>> GetOrderItems(int orderId)
        {
            if (orderId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderId));
            }

            IDictionary<int, int> items = new Dictionary<int, int>();

            using (IDataReader reader = await this._dataAccessAdapter.OrderAdapter.GetOrderItems(orderId))
            {
                while (reader.Read())
                {
                    int paintingId = (int)reader["paintingId"];
                    int count = (int)reader["count"];

                    items.Add(paintingId, count);
                }
            }

            return items;
        }

        public async Task PayOrder(int orderId)
        {
            if (orderId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderId));
            }

            await this._dataAccessAdapter.OrderAdapter.PayOrder(orderId);
        }

        private Order ConvertReaderToOrder(IDataReader reader)
        {
            return new Order
            {
                OrderId = (int)reader["orderId"],
                UserId = (int)reader["userId"],
                Date = Convert.ToDateTime(reader["date"]),
                Amount = (double)reader["amount"],
                Status = (bool)reader["status"],
            };
        }
    }
}
