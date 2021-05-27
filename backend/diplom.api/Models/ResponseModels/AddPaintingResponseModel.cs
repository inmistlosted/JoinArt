using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Models.ResponseModels
{
    public class AddPaintingResponseModel
    {
        public int PaintingId { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
