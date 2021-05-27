using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Models.RequestModels
{
    public class OrderItemRequestModel
    {
        public int PaintingId { get; set; }
        public int Count { get; set; }
    }
}
