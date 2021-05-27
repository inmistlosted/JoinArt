using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer
{
    public interface IOrderAdapter
    {
        Task<IDataReader> CreateOrder(int userId, DateTime date, double amount, bool status);
        Task AddOrderItem(int orderId, int paintingId, int count);
        Task<IDataReader> GetUserOrders(int userId);
        Task<IDataReader> GetOrderItems(int orderId);
        Task PayOrder(int orderId);
    }
}
