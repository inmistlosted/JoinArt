using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Models.ResponseModels
{
    public class GetBidResponseModel
    {
        public int BidId { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
