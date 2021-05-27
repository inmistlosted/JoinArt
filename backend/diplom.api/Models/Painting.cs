using System;

namespace diplom.api.Models
{
    public class Painting
    {
        public int Id { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public int ViewsCount { get; set; }
        public int BetsCount { get; set; }
        public double Price { get; set; }
        public bool Status { get; set; }
        public string Title { get; set; }
        public string Materials { get; set; }
        public string Painter { get; set; }
        public string Description { get; set; }
        public DateTime UploadDate { get; set; }
        public string ImagePath { get; set; }
    }
}
