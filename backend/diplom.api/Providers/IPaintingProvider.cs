using diplom.api.Models;
using diplom.api.Models.RequestModels;
using diplom.api.Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace diplom.api.Providers
{
    public interface IPaintingProvider
    {
        Task<IList<PaintingResponseModel>> GetAll(int userId, bool withCache = false);
        Task<PaintingResponseModel> GetPainting(int id, int userId, bool withCache = false);
        Task<AddPaintingResponseModel> AddPainting(AddPaintingRequestModel painting);
        Task<AddPaintingResponseModel> UpdatePainting(AddPaintingRequestModel painting);
        Task<LikeResponseModel> AddLike(int paintingId, int userId);
        Task RemoveLike(int paintingId, int userId);
        Task<CommentResponseModel> AddComment(CommentRequestModel comment);
        Task<IList<PaintingResponseModel>> GetPaintingsOfGenre(int genreId, int userId, bool withCache = false);
        Task<IList<PaintingResponseModel>> GetPaintingsOfAlbum(int albumId, int userId, bool withCache = false);
        Task<bool> IsPaintingInAlbum(int paintingId, int albumId);
        Task<IList<PaintingResponseModel>> SearchPaintings(string query, int userId, bool withCache = false);
        Task<double> GetPaintingStartPrice(int id);
        Task UpdatePaintingPrice(int id, double price);
        Task<IList<PaintingResponseModel>> GetTopPaintings(int userId, int take, bool withCache = false);
        Task<IList<PaintingResponseModel>> GetPaintingsOfUser(int userId,int currentUserId, bool withCache = false);
        Task DeletePainting(int paintingId);
    }
}
