using diplom.api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Providers
{
    public interface IOrderProvider
    {
        Task<int> CreateOrder(int userId, double amount);
        Task AddOrderItem(int orderId, int paintingId, int count);
        Task<IList<Order>> GetUserOrders(int userId);
        Task<IDictionary<int, int>> GetOrderItems(int orderId);
        Task PayOrder(int orderId);
    }
}
