using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diplom.api.DataAccessLayer;
using diplom.api.Models;
using diplom.api.Models.RequestModels;
using diplom.api.Models.ResponseModels;
using diplom.api.Providers;
using diplom.api.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace diplom.api.Controllers
{
    [Route("albums")]
    public class AlbumsController : BaseController
    {
        private const int TopAlbumsTake = 6;

        private readonly IAlbumProvider _albumProvider;
        private readonly IPaintingProvider _paintingProvider;

        public AlbumsController(IDataAccessAdapter dataAccessAdapter, IMemoryCache cache, IOptions<AppSettings> settings, IAlbumProvider albumProvider, IPaintingProvider paintingProvider)
            : base(dataAccessAdapter, cache, settings)
        {
            this._albumProvider = albumProvider ?? throw new ArgumentNullException(nameof(albumProvider));
            this._paintingProvider = paintingProvider ?? throw new ArgumentNullException(nameof(paintingProvider));
        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> Create(CreateAlbumRequestModel model)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            CreateAlbumResponseModel response = await _albumProvider.Create(model);

            return Json(response);
        }

        [HttpPost, Route("update")]
        public async Task<IActionResult> Update(CreateAlbumRequestModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            await _albumProvider.Update(model);

            return Ok();
        }

        [HttpPost, Route("delete/{albumId}")]
        public async Task<IActionResult> Delete(int albumId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            await _albumProvider.Delete(albumId);

            return Ok();
        }

        [HttpGet, Route("get-top-albums")]
        public async Task<IActionResult> GetTopAlbums()
        {
            IList<Album> topAlbums = await _albumProvider.GetTopAlbums(TopAlbumsTake);

            return Json(topAlbums);
        }

        [HttpGet, Route("get-all-albums")]
        public async Task<IActionResult> GetAllAlbums()
        {
            IList<Album> albums = await _albumProvider.GetAllAlbums();

            return Json(albums);
        }

        [HttpGet, Route("get-user-albums/{userId}/{paintingId}")]
        public async Task<IActionResult> GetUserAlbums(int userId, int paintingId)
        {
            if(userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            IList<Album> albums = await _albumProvider.GetUserAlbums(userId);

            foreach(Album album in albums)
            {
                album.IsPaintingInAlbum = await _paintingProvider.IsPaintingInAlbum(paintingId, album.AlbumId);
            }

            return Json(albums);
        }

        [HttpGet, Route("get-user-albums/{userId}")]
        public async Task<IActionResult> GetUserAlbums(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            IList<Album> albums = await _albumProvider.GetUserAlbums(userId);

            return Json(albums);
        }

        [HttpPost, Route("add-painting-to-album/{albumId}/{paintingId}")]
        public async Task<IActionResult> AddPaintingToAlbum(int albumId, int paintingId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            AddPaintingToAlbumModel response = await _albumProvider.AddPaintingToAlbum(albumId, paintingId);

            return Json(response);
        }

        [HttpPost, Route("remove-painting-from-album/{albumId}/{paintingId}")]
        public async Task<IActionResult> RemovePaintingFromAlbum(int albumId, int paintingId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            await _albumProvider.RemovePaintingFromAlbum(albumId, paintingId);

            return Ok();
        }

        [HttpGet, Route("get-album/{albumId}/{userId}")]
        public async Task<IActionResult> GetAlbum(int albumId, int userId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            Album album = await _albumProvider.GetAlbum(albumId, userId);

            if(album != null)
            {
                album.Paintings = await _paintingProvider.GetPaintingsOfAlbum(albumId, userId);
                album.Image = album.Paintings.FirstOrDefault()?.ImagePath;
            }

            return Json(album);
        }
    }
}
