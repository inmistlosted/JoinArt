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
    [Route("paintings")]
    public class PaintingsController : BaseController
    {
        private const int TopPaintingsTake = 10;

        private readonly IPaintingProvider _paintingProvider;
        private readonly IGenreProvider _genreProvider;
        private readonly IUserProvider _userProvider;

        public PaintingsController(
            IDataAccessAdapter dataAccessAdapter, 
            IMemoryCache cache, 
            IOptions<AppSettings> settings, 
            IPaintingProvider paintingProvider, 
            IGenreProvider genreProvider, 
            IUserProvider userProvider)
            : base(dataAccessAdapter, cache, settings)
        {
            this._paintingProvider = paintingProvider ?? throw new ArgumentNullException(nameof(paintingProvider));
            this._genreProvider = genreProvider ?? throw new ArgumentNullException(nameof(genreProvider));
            this._userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        [HttpPost, Route("add-painting")]
        public async Task<IActionResult> AddPainting(AddPaintingRequestModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            AddPaintingResponseModel response = await _paintingProvider.AddPainting(model);

            return Json(response);
        }

        [HttpPost, Route("update-painting")]
        public async Task<IActionResult> UpdatePainting(AddPaintingRequestModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            AddPaintingResponseModel response = await _paintingProvider.UpdatePainting(model);

            return Json(response);
        }

        [HttpGet, Route("get-all/{userId}")]
        public async Task<IActionResult> GetAll(int userId)
        {
            if(userId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            IList<PaintingResponseModel> paintings = await this._paintingProvider.GetAll(userId);

            return Json(paintings);
        }

        [HttpGet, Route("get-top-paintings/{userId}")]
        public async Task<IActionResult> GetTopPaintings(int userId)
        {
            if (userId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            IList<PaintingResponseModel> paintings = await this._paintingProvider.GetTopPaintings(userId, TopPaintingsTake);

            return Json(paintings);
        }

        [HttpGet, Route("get-painting/{paintingId}/{userId}")]
        public async Task<IActionResult> GetPainting(int paintingId, int userId)
        {
            if(paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            PaintingResponseModel painting = await _paintingProvider.GetPainting(paintingId, userId);

            if(painting == null)
            {
                return Json(null);
            }

            IList<Genre> genres = await _genreProvider.GetPaintingGenres(paintingId);
            User owner = await _userProvider.GetPaintingOwner(paintingId);

            painting.Genres = genres;
            painting.Owner = owner;

            if (userId != 0 && owner != null)
            {
                painting.IsFollowingOwner = await _userProvider.IsFollowing(userId, owner.UserId);
            }
            
            return Json(painting);
        }

        [HttpPost, Route("add-like/{paintingId}/{userId}")]
        public async Task<IActionResult> AddLike(int paintingId, int userId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            LikeResponseModel response = await _paintingProvider.AddLike(paintingId, userId);

            return Json(response);
        }

        [HttpPost, Route("remove-like/{paintingId}/{userId}")]
        public async Task<IActionResult> RemoveLike(int paintingId, int userId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            await _paintingProvider.RemoveLike(paintingId, userId);

            return Ok();
        }

        [HttpPost, Route("add-comment")]
        public async Task<IActionResult> AddComment(CommentRequestModel model)
        {
            if (model == null)
            {
                throw new ArgumentOutOfRangeException(nameof(model));
            }

            CommentResponseModel response = await _paintingProvider.AddComment(model);

            return Json(response);
        }

        [HttpGet, Route("search/{query}/{userId}")]
        public async Task<IActionResult> SearchPaintings(string query, int userId)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            IList<PaintingResponseModel> paintings = await this._paintingProvider.SearchPaintings(query, userId);

            return Json(paintings);
        }

        [HttpGet, Route("get-user-paintings/{userId}/{currentUserId}")]
        public async Task<IActionResult> GetUserPaintings(int userId, int currentUserId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            if (currentUserId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(currentUserId));
            }

            IList<PaintingResponseModel> paintings = await this._paintingProvider.GetPaintingsOfUser(userId, currentUserId);

            return Json(paintings);
        }

        [HttpPost, Route("delete-painting/{paintingId}")]
        public async Task<IActionResult> DeletePainting(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            await _paintingProvider.DeletePainting(paintingId);

            return Ok();
        }
    }
}
