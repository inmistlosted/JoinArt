using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Models.RequestModels
{
    public class CommentRequestModel
    {
        public int PaintingId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
    }
}
