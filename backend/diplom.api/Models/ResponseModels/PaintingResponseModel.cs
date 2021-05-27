using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Models.ResponseModels
{
    public class PaintingResponseModel
    {
        public int PaintingId { get; set; }
        public double Price { get; set; }
        public double Rating { get; set; }
        public bool Status { get; set; }
        public bool IsLiked { get; set;}
        public bool IsBidProduct { get; set; }
        public bool IsFollowingOwner { get; set; }
        public string Title { get; set; }
        public string Materials { get; set; }
        public string Painter { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public string ImagePath { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public IList<Comment> Comments { get; set; }
        public IList<Genre> Genres { get; set; }
        public User Owner { get; set; }
    }
}
