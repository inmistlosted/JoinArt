using diplom.api.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Models
{
    public class OrderItem
    {
        public PaintingResponseModel Painting { get; set; }
        public int Count { get; set; }
    }
}
