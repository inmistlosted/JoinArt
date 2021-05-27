using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Models
{
    public class BidHistoryItem
    {
        public int BetId { get; set; }
        public string UserLogin { get; set; }
        public double Bet { get; set; }
        public DateTime PlaceBetTime { get; set; }
    }
}
