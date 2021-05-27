using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Models.RequestModels
{
    public class AddPaintingRequestModel
    {
        public int PaintingId { get; set; }
        public int OwnerId { get; set; }
        public string Title { get; set; }
        public string Materials { get; set; }
        public string Painter { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public IFormFile Image { get; set; }
        public string GenresIds { get; set; }
    }
}
