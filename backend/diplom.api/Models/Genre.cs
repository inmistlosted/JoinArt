using diplom.api.Models.ResponseModels;
using System.Collections.Generic;

namespace diplom.api.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public long LikesCount { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsMovement { get; set; }
        public string Image { get; set; }
        public IList<PaintingResponseModel> Paintings { get; set; }
    }
}
