using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Models.RequestModels
{
    public class CreateOrderRequestModel
    {
        public int UserId { get; set; }
        public IList<OrderItemRequestModel> Paintings { get; set; }
    }
}
