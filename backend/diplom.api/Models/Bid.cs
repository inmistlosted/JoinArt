using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace diplom.api.Models
{
    public class Bid
    {
        public int BidId { get; set; }
        public int PaintingId { get; set; }
        public long LikesCount { get; set; }
        public bool Status { get; set; }
        public bool HasStarted { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string PaintingImage { get; set; }
        public string PaintingTitle { get; set; }
        public IList<BidHistoryItem> History {get ;set;}

    }
}
