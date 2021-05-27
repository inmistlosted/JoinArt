using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer
{
    public interface IPaintingAdapter
    {
        Task<IDataReader> GetAll();
        Task<IDataReader> GetPainting(int paintingId);
        Task<IDataReader> GetPaintingComments(int paintingId);
        Task<IDataReader> GetPaintingCommentsCount(int paintingId);
        Task<IDataReader> GetPaintingLikesCount(int paintingId);
        Task<IDataReader> IsPaintingLiked(int paintingId, int userId);
        Task<IDataReader> IsBidProduct(int paintingId);
        Task<IDataReader> AddPainting(string title, string materials, string painter, double price, string description, DateTime uploadDate, bool status, byte[] image);
        Task<IDataReader> UpdatePainting(int id, string title, string materials, string painter, double price, string description, bool status, byte[] image);
        Task AddPaintingOwner(int paintingId, int ownerId);
        Task AddPaintingGenres(int paintingId, IList<int> genresIds);
        Task RemovePaintingGenres(int paintingId);
        Task<IDataReader> AddLike(int paintingId, int userId);
        Task RemoveLike(int paintingId, int userId);
        Task<IDataReader> AddComment(int paintingId, int userId, string content, DateTime date);
        Task<IDataReader> GetPaintingsOfGenre(int genreId);
        Task<IDataReader> GetPaintingsOfAlbum(int albumId);
        Task<IDataReader> GetPaintingsOfUser(int userId);
        Task<IDataReader> IsPaintingInAlbum(int paintingId, int albumId);
        Task<IDataReader> SearchPaintings(string query);
        Task<IDataReader> GetPaintingPrice(int paintingId);
        Task UpdatePaintingPrice(int paintingId, double price);
        Task DeletePainting(int paintingId);
    }
}
