using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer
{
    public interface IAlbumAdapter
    {
        Task<IDataReader> Create(string title, string description, int userId);
        Task Update(int albumId, string title, string description);
        Task Delete(int albumId);
        Task<IDataReader> GetAlbumsOfUser(int userId);
        Task<IDataReader> GetAlbum(int albumId);
        Task<IDataReader> GetAllAlbums();
        Task<IDataReader> GetAlbumLikesCount(int albumId);
        Task<IDataReader> GetAlbumImage(int albumId);
        Task<IDataReader> AddPaintingToAlbum(int albumId, int paintingId);
        Task<IDataReader> RemovePaintingFromAlbum(int albumId, int paintingId);
        Task<IDataReader> IsAlbumBelongsToUser(int albumId, int userId);
    }
}
