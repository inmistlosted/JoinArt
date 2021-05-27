using diplom.api.Classes;
using diplom.api.DataAccessLayer;
using diplom.api.Models;
using diplom.api.Models.RequestModels;
using diplom.api.Models.ResponseModels;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Providers.Implementation
{
    public class PaintingProvider : IPaintingProvider
    {
        private readonly IDataAccessAdapter _dataAccessAdapter;
        private readonly IMemoryCache _cache;

        public PaintingProvider(IDataAccessAdapter dataAccessAdapter, IMemoryCache cache)
        {
            this._dataAccessAdapter = dataAccessAdapter ?? throw new ArgumentNullException(nameof(dataAccessAdapter));
            this._cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IList<PaintingResponseModel>> GetAll(int userId, bool withCache = false)
        {
            string cacheKey = $"paintings_get_all_{userId}";

            IList<PaintingResponseModel> paintings = withCache ? Helper.GetFromCache<IList<PaintingResponseModel>>(this._cache, cacheKey) : null;

            if (paintings == null)
            {
                IList<Painting> paintingsInfos = new List<Painting>();
                paintings = new List<PaintingResponseModel>();

                using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.GetAll())
                {
                    while (reader.Read())
                    {
                        paintingsInfos.Add(ConvertReaderToPainting(reader));
                    }
                }

                foreach (Painting paintingInfo in paintingsInfos)
                {
                    int likesCount = await GetLikesCount(paintingInfo.Id);
                    int commentsCount = await GetCommentsCount(paintingInfo.Id);
                    bool isPaintingLiked = userId == 0 ? false : await IsPaintingLiked(paintingInfo.Id, userId);

                    paintings.Add(new PaintingResponseModel
                    {
                        PaintingId = paintingInfo.Id,
                        Price = paintingInfo.Price,
                        Status = paintingInfo.Status,
                        Title = paintingInfo.Title,
                        Materials = paintingInfo.Materials,
                        Painter = paintingInfo.Painter,
                        Description = paintingInfo.Description,
                        UploadDate = paintingInfo.UploadDate,
                        ImagePath = paintingInfo.ImagePath,
                        CommentsCount = commentsCount,
                        LikesCount = likesCount,
                        IsLiked = isPaintingLiked,
                    });
                }

                Helper.SetToCache(this._cache, cacheKey, paintings);
            }

            return paintings;
        }

        public async Task<IList<PaintingResponseModel>> GetTopPaintings(int userId, int take, bool withCache = false)
        {
            string cacheKey = $"get_top_paintings_{userId}_{take}";

            IList<PaintingResponseModel> paintings = withCache ? Helper.GetFromCache<IList<PaintingResponseModel>>(this._cache, cacheKey) : null;

            if (paintings == null)
            {
                IList<PaintingResponseModel> allPaintings = await GetAll(userId);

                if(allPaintings != null && allPaintings.Any())
                {
                    paintings = allPaintings.OrderByDescending(x => x.LikesCount).Take(take).ToList();

                    Helper.SetToCache(this._cache, cacheKey, paintings);
                }
            }

            return paintings;
        }

        public async Task<PaintingResponseModel> GetPainting(int id, int userId, bool withCache = false)
        {
            if(id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            string cacheKey = $"get_painting_{id}_{userId}";

            PaintingResponseModel painting = withCache ? Helper.GetFromCache<PaintingResponseModel>(this._cache, cacheKey) : null;

            if(painting == null)
            {
                Painting paintingInfo = await GetPaintingInfo(id);

                if(paintingInfo != null)
                {
                    IList<Comment> comments = await GetComments(id);
                    int likesCount = await GetLikesCount(id);
                    bool isPaintingLiked = userId == 0 ? false : await IsPaintingLiked(id, userId);
                    bool isBidProduct = await IsBidProduct(id);

                    painting = new PaintingResponseModel
                    {
                        PaintingId = paintingInfo.Id,
                        Price = paintingInfo.Price,
                        Status = paintingInfo.Status,
                        Title = paintingInfo.Title,
                        Materials = paintingInfo.Materials,
                        Painter = paintingInfo.Painter,
                        Description = paintingInfo.Description,
                        UploadDate = paintingInfo.UploadDate,
                        ImagePath = paintingInfo.ImagePath,
                        Comments = comments,
                        CommentsCount = comments.Count,
                        LikesCount = likesCount,
                        IsLiked = isPaintingLiked,
                        IsBidProduct = isBidProduct,
                    };

                    Helper.SetToCache(this._cache, cacheKey, painting);
                }
                
            }

            return painting;
        }

        public async Task<AddPaintingResponseModel> AddPainting(AddPaintingRequestModel painting)
        {
            if(painting == null)
            {
                throw new ArgumentNullException(nameof(painting));
            }

            byte[] image = Helper.FileAsBytes(painting.Image);
            int paintingId = 0;
            bool isAdded = false;

            using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.AddPainting(
                painting.Title, 
                painting.Materials, 
                painting.Painter, 
                painting.Price, 
                painting.Description, 
                DateTime.Now, 
                true, 
                image))
            {
                if (reader.Read())
                {
                    paintingId = (int)reader["paintingId"];
                    isAdded = paintingId != 0;
                }
            }

            if (isAdded)
            {
                IList<int> genresIds = Helper.GetListFromString(painting.GenresIds);

                await this._dataAccessAdapter.PaintingAdapter.AddPaintingOwner(paintingId, painting.OwnerId);
                await this._dataAccessAdapter.PaintingAdapter.AddPaintingGenres(paintingId, genresIds);
            }

            AddPaintingResponseModel response = new AddPaintingResponseModel
            {
                Status = isAdded,
                Message = isAdded ? string.Empty : "Unexpected error occurred"
            };

            return response;
        }

        public async Task<AddPaintingResponseModel> UpdatePainting(AddPaintingRequestModel painting)
        {
            if (painting == null)
            {
                throw new ArgumentNullException(nameof(painting));
            }

            byte[] image = Helper.FileAsBytes(painting.Image);
            bool isUpdated = false;

            using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.UpdatePainting(
                painting.PaintingId,
                painting.Title,
                painting.Materials,
                painting.Painter,
                painting.Price,
                painting.Description,
                true, 
                image))
            {
                if (reader.Read())
                {
                    isUpdated = (int)reader["paintingId"] == painting.PaintingId;
                }
            }

            if (isUpdated)
            {
                IList<int> genresIds = Helper.GetListFromString(painting.GenresIds);

                await this._dataAccessAdapter.PaintingAdapter.RemovePaintingGenres(painting.PaintingId);
                await this._dataAccessAdapter.PaintingAdapter.AddPaintingGenres(painting.PaintingId, genresIds);
            }

            AddPaintingResponseModel response = new AddPaintingResponseModel
            {
                Status = isUpdated,
                Message = isUpdated ? string.Empty : "Unexpected error occurred"
            };

            return response;
        }

        public async Task<LikeResponseModel> AddLike(int paintingId, int userId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            bool isAdded = false;

            if(!(await IsPaintingLiked(paintingId, userId)))
            {
                using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.AddLike(paintingId, userId))
                {
                    if (reader.Read())
                    {
                        isAdded = (int)reader["likeId"] != 0;
                    }
                }
            }

            LikeResponseModel response = new LikeResponseModel
            {
                IsAdded = isAdded,
                Message = isAdded ? string.Empty : "Unexpected error occurred",
            };

            return response;
        }

        public async Task RemoveLike(int paintingId, int userId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            await this._dataAccessAdapter.PaintingAdapter.RemoveLike(paintingId, userId);
        }

        public async Task<CommentResponseModel> AddComment(CommentRequestModel comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment));
            }

            bool isAdded = false;
            DateTime date = DateTime.Now;

            using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.AddComment(comment.PaintingId, comment.UserId, comment.Content, date))
            {
                if (reader.Read())
                {
                    isAdded = (int)reader["commentId"] != 0;
                }
            }

            CommentResponseModel response = new CommentResponseModel
            {
                IsAdded = isAdded,
                Message = isAdded ? string.Empty : "Unexpected error occurred",
            };

            return response;
        }

        public async Task<IList<PaintingResponseModel>> GetPaintingsOfGenre(int genreId, int userId, bool withCache = false)
        {
            if(genreId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(genreId));
            }

            string cacheKey = $"get_paintings_of_genre_{genreId}_{userId}";

            IList<PaintingResponseModel> paintings = withCache ? Helper.GetFromCache<IList<PaintingResponseModel>>(this._cache, cacheKey) : null;

            if (paintings == null)
            {
                IList<Painting> paintingsInfos = new List<Painting>();
                paintings = new List<PaintingResponseModel>();

                using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.GetPaintingsOfGenre(genreId))
                {
                    while (reader.Read())
                    {
                        paintingsInfos.Add(ConvertReaderToPainting(reader));
                    }
                }

                foreach(Painting paintingInfo in paintingsInfos)
                {
                    int likesCount = await GetLikesCount(paintingInfo.Id);
                    int commentsCount = await GetCommentsCount(paintingInfo.Id);
                    bool isPaintingLiked = userId == 0 ? false : await IsPaintingLiked(paintingInfo.Id, userId);
                    double rating = Helper.CalculatePaintingRating(paintingInfo);

                    paintings.Add(new PaintingResponseModel
                    {
                        PaintingId = paintingInfo.Id,
                        Price = paintingInfo.Price,
                        Status = paintingInfo.Status,
                        Title = paintingInfo.Title,
                        Materials = paintingInfo.Materials,
                        Painter = paintingInfo.Painter,
                        Description = paintingInfo.Description,
                        UploadDate = paintingInfo.UploadDate,
                        ImagePath = paintingInfo.ImagePath,
                        CommentsCount = commentsCount,
                        LikesCount = likesCount,
                        IsLiked = isPaintingLiked,
                        Rating = rating,
                    });
                }

                Helper.SetToCache(this._cache, cacheKey, paintings);
            }

            return paintings;
        }

        public async Task<IList<PaintingResponseModel>> GetPaintingsOfAlbum(int albumId, int userId, bool withCache = false)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            string cacheKey = $"get_paintings_of_album_{albumId}_{userId}";

            IList<PaintingResponseModel> paintings = withCache ? Helper.GetFromCache<IList<PaintingResponseModel>>(this._cache, cacheKey) : null;

            if (paintings == null)
            {
                IList<Painting> paintingsInfos = new List<Painting>();
                paintings = new List<PaintingResponseModel>();

                using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.GetPaintingsOfAlbum(albumId))
                {
                    while (reader.Read())
                    {
                        paintingsInfos.Add(ConvertReaderToPainting(reader));
                    }
                }

                foreach (Painting paintingInfo in paintingsInfos)
                {
                    int likesCount = await GetLikesCount(paintingInfo.Id);
                    int commentsCount = await GetCommentsCount(paintingInfo.Id);
                    bool isPaintingLiked = userId == 0 ? false : await IsPaintingLiked(paintingInfo.Id, userId);

                    paintings.Add(new PaintingResponseModel
                    {
                        PaintingId = paintingInfo.Id,
                        Price = paintingInfo.Price,
                        Status = paintingInfo.Status,
                        Title = paintingInfo.Title,
                        Materials = paintingInfo.Materials,
                        Painter = paintingInfo.Painter,
                        Description = paintingInfo.Description,
                        UploadDate = paintingInfo.UploadDate,
                        ImagePath = paintingInfo.ImagePath,
                        CommentsCount = commentsCount,
                        LikesCount = likesCount,
                        IsLiked = isPaintingLiked,
                    });
                }

                Helper.SetToCache(this._cache, cacheKey, paintings);
            }

            return paintings;
        }

        public async Task<IList<PaintingResponseModel>> GetPaintingsOfUser(int userId, int currentUserId, bool withCache = false)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            string cacheKey = $"get_paintings_of_user_{userId}_{currentUserId}";

            IList<PaintingResponseModel> paintings = withCache ? Helper.GetFromCache<IList<PaintingResponseModel>>(this._cache, cacheKey) : null;

            if (paintings == null)
            {
                IList<Painting> paintingsInfos = new List<Painting>();
                paintings = new List<PaintingResponseModel>();

                using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.GetPaintingsOfUser(userId))
                {
                    while (reader.Read())
                    {
                        paintingsInfos.Add(ConvertReaderToPainting(reader));
                    }
                }

                foreach (Painting paintingInfo in paintingsInfos)
                {
                    int likesCount = await GetLikesCount(paintingInfo.Id);
                    int commentsCount = await GetCommentsCount(paintingInfo.Id);
                    bool isPaintingLiked = currentUserId == 0 ? false : await IsPaintingLiked(paintingInfo.Id, currentUserId);

                    paintings.Add(new PaintingResponseModel
                    {
                        PaintingId = paintingInfo.Id,
                        Price = paintingInfo.Price,
                        Status = paintingInfo.Status,
                        Title = paintingInfo.Title,
                        Materials = paintingInfo.Materials,
                        Painter = paintingInfo.Painter,
                        Description = paintingInfo.Description,
                        UploadDate = paintingInfo.UploadDate,
                        ImagePath = paintingInfo.ImagePath,
                        CommentsCount = commentsCount,
                        LikesCount = likesCount,
                        IsLiked = isPaintingLiked,
                    });
                }

                Helper.SetToCache(this._cache, cacheKey, paintings);
            }

            return paintings;
        }

        public async Task<bool> IsPaintingInAlbum(int paintingId, int albumId)
        {
            using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.IsPaintingInAlbum(paintingId, albumId))
            {
                if (reader.Read())
                {
                    return (int)reader["paintingId"] != 0;
                }
            }

            return false;
        }

        public async Task<IList<PaintingResponseModel>> SearchPaintings(string query, int userId, bool withCache = false)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            string cacheKey = $"search_paintings_{query}_{userId}";

            IList<PaintingResponseModel> paintings = withCache ? Helper.GetFromCache<IList<PaintingResponseModel>>(this._cache, cacheKey) : null;

            if (paintings == null)
            {
                IList<Painting> paintingsInfos = new List<Painting>();
                paintings = new List<PaintingResponseModel>();

                using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.SearchPaintings(query.ToLower()))
                {
                    while (reader.Read())
                    {
                        paintingsInfos.Add(ConvertReaderToPainting(reader));
                    }
                }

                foreach (Painting paintingInfo in paintingsInfos)
                {
                    int likesCount = await GetLikesCount(paintingInfo.Id);
                    int commentsCount = await GetCommentsCount(paintingInfo.Id);
                    bool isPaintingLiked = userId == 0 ? false : await IsPaintingLiked(paintingInfo.Id, userId);

                    paintings.Add(new PaintingResponseModel
                    {
                        PaintingId = paintingInfo.Id,
                        Price = paintingInfo.Price,
                        Status = paintingInfo.Status,
                        Title = paintingInfo.Title,
                        Materials = paintingInfo.Materials,
                        Painter = paintingInfo.Painter,
                        Description = paintingInfo.Description,
                        UploadDate = paintingInfo.UploadDate,
                        ImagePath = paintingInfo.ImagePath,
                        CommentsCount = commentsCount,
                        LikesCount = likesCount,
                        IsLiked = isPaintingLiked,
                    });
                }

                Helper.SetToCache(this._cache, cacheKey, paintings);
            }

            return paintings;
        }

        public async Task<double> GetPaintingStartPrice(int id)
        {
            using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.GetPaintingPrice(id))
            {
                if (reader.Read())
                {
                    return (double)reader["price"];
                }
            }

            return 0;
        }

        public async Task UpdatePaintingPrice(int id, double price)
        {
            if(id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            if(price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price));
            }

            await _dataAccessAdapter.PaintingAdapter.UpdatePaintingPrice(id, price);
        }

        public async Task DeletePainting(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            await this._dataAccessAdapter.PaintingAdapter.DeletePainting(paintingId);
        }

        private async Task<Painting> GetPaintingInfo(int id)
        {
            using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.GetPainting(id))
            {
                if (reader.Read())
                {
                    return ConvertReaderToPainting(reader);
                }
            }

            return null;
        }

        private async Task<IList<Comment>> GetComments(int paintingId)
        {
            IList<Comment> comments = new List<Comment>();

            using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.GetPaintingComments(paintingId))
            {
                while (reader.Read())
                {
                    comments.Add(new Comment { 
                        Id = (int)reader["commentId"],
                        UserName = reader["login"].ToString(),
                        Content = reader["content"].ToString(),
                        Date = Convert.ToDateTime(reader["date"]),
                    });
                }
            }

            return comments;
        }

        private async Task<int> GetCommentsCount(int paintingId)
        {
            using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.GetPaintingCommentsCount(paintingId))
            {
                if (reader.Read())
                {
                    return Convert.ToInt32(reader["count"]);
                }
            }

            return 0;
        }

        private async Task<int> GetLikesCount(int paintingId)
        {
            using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.GetPaintingLikesCount(paintingId))
            {
                if (reader.Read())
                {
                    return Convert.ToInt32(reader["count"]);
                }
            }

            return 0;
        }

        private async Task<bool> IsPaintingLiked(int paintingId, int userId)
        {
            using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.IsPaintingLiked(paintingId, userId))
            {
                if (reader.Read())
                {
                    return (int)reader["likeId"] != 0;
                }
            }

            return false;
        }

        private async Task<bool> IsBidProduct(int paintingId)
        {
            using (IDataReader reader = await this._dataAccessAdapter.PaintingAdapter.IsBidProduct(paintingId))
            {
                if (reader.Read())
                {
                    return (int)reader["bidId"] != 0;
                }
            }

            return false;
        }

        private Painting ConvertReaderToPainting(IDataReader reader)
        {
            return new Painting {
                Id = (int)reader["PaintingId"],
                Title = reader["Title"].ToString(),
                Materials = reader["Materials"] == DBNull.Value ? null : reader["Materials"].ToString(),
                Painter = reader["Painter"].ToString(),
                Price = (double)reader["Price"],
                Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                UploadDate = Convert.ToDateTime(reader["UploadDate"]),
                Status = (bool)reader["Status"],
                ImagePath = Helper.ToRenderablePictureString(reader["Image"] == DBNull.Value ? null : (byte[])reader["Image"]),
            };
        }
    }
}
