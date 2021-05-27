using diplom.api.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Models
{
    public class Album
    {
        public double Rating { get; set; }
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public long LikesCount { get; set; }
        public string Image { get; set; }
        public bool IsPaintingInAlbum { get; set; }
        public bool BelongsToUser { get; set; }
        public IList<PaintingResponseModel> Paintings { get; set; }
    }
}
