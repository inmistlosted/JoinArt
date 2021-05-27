using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public bool Status { get; set; }
        public DateTime Date { get; set; }
        public IList<OrderItem> OrderItems { get; set; }
        public OrderItem OrderItem { get; set; }
    }
}
