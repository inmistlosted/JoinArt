using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diplom.api.DataAccessLayer;
using diplom.api.Models;
using diplom.api.Models.RequestModels;
using diplom.api.Models.ResponseModels;
using diplom.api.Providers;
using diplom.api.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace diplom.api.Controllers
{
    [Route("orders")]
    public class OrdersController : BaseController
    {
        private readonly IOrderProvider _orderProvider;
        private readonly IPaintingProvider _paintingProvider;

        public OrdersController(IDataAccessAdapter dataAccessAdapter, IMemoryCache cache, IOptions<AppSettings> settings, IOrderProvider orderProvider, IPaintingProvider paintingProvider)
            : base(dataAccessAdapter, cache, settings)
        {
            this._orderProvider = orderProvider ?? throw new ArgumentNullException(nameof(orderProvider));
            this._paintingProvider = paintingProvider ?? throw new ArgumentNullException(nameof(paintingProvider));
        }

        [HttpPost, Route("create-order")]
        public async Task<IActionResult> CreateOrder(CreateOrderRequestModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if(model.Paintings == null || !model.Paintings.Any())
            {
                return BadRequest();
            }

            double amount = await GetOrderAmount(model.Paintings.Select(x => x.PaintingId));

            int newOrderId = await _orderProvider.CreateOrder(model.UserId, amount);

            foreach(OrderItemRequestModel item in model.Paintings)
            {
                await _orderProvider.AddOrderItem(newOrderId, item.PaintingId, item.Count);
            }

            return Ok();
        }

        [HttpGet, Route("get-user-orders/{userId}")]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            IList<Order> orders = await _orderProvider.GetUserOrders(userId);

            foreach(Order order in orders)
            {
                IDictionary<int, int> orderItemsIds = await _orderProvider.GetOrderItems(order.OrderId);

                IList<OrderItem> orderItems = new List<OrderItem>();

                foreach(KeyValuePair<int, int> orderItemId in orderItemsIds)
                {
                    PaintingResponseModel painting = await _paintingProvider.GetPainting(orderItemId.Key, 0);

                    orderItems.Add(new OrderItem {
                        Painting = painting,
                        Count = orderItemId.Value,
                    });
                }

                order.OrderItems = orderItems;
                order.OrderItem = orderItems.First();
            }

            return Json(orders);
        }

        [HttpPost, Route("pay-order/{orderId}")]
        public async Task<IActionResult> PayOrder(int orderId)
        {
            if (orderId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(orderId));
            }

            await _orderProvider.PayOrder(orderId);

            return Ok();
        }

        private async Task<double> GetOrderAmount(IEnumerable<int> paintingsIds)
        {
            double amount = 0;

            foreach(int paintingId in paintingsIds)
            {
                PaintingResponseModel painting = await _paintingProvider.GetPainting(paintingId, 0);

                amount += painting.Price;
            }

            return amount;
        }
    }
}
