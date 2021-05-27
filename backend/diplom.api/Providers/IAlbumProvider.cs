using diplom.api.Models;
using diplom.api.Models.RequestModels;
using diplom.api.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Providers
{
    public interface IAlbumProvider
    {
        Task<CreateAlbumResponseModel> Create(CreateAlbumRequestModel album);
        Task Update(CreateAlbumRequestModel album);
        Task Delete(int albumId);
        Task<Album> GetAlbum(int albumId, int userId, bool withCache = false);
        Task<IList<Album>> GetUserAlbums(int userId, bool withCache = false);
        Task<IList<Album>> GetTopAlbums(int take, bool withCache = false);
        Task<IList<Album>> GetAllAlbums(bool withCache = false);
        Task<AddPaintingToAlbumModel> AddPaintingToAlbum(int albumId, int paintingId);
        Task RemovePaintingFromAlbum(int albumId, int paintingId);
    }
}
