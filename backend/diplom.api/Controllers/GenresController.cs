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
    [Route("genres")]
    public class GenresController : BaseController
    {
        private const int TopGenresTake = 6;

        private readonly IGenreProvider _genreProvider;
        private readonly IPaintingProvider _paintingProvider;

        public GenresController(IDataAccessAdapter dataAccessAdapter, IMemoryCache cache, IOptions<AppSettings> settings, IGenreProvider genreProvider, IPaintingProvider paintingProvider)
            : base(dataAccessAdapter, cache, settings)
        {
            this._genreProvider = genreProvider ?? throw new ArgumentNullException(nameof(genreProvider));
            this._paintingProvider = paintingProvider ?? throw new ArgumentNullException(nameof(paintingProvider));
        }

        [HttpPost, Route("add-genre")]
        public async Task<IActionResult> AddGenre(GenreRequestModel genre)
        {
            GenreResponseModel response = await _genreProvider.AddGenre(genre);

            return Json(response);
        }

        [HttpPut, Route("update-genre")]
        public async Task<IActionResult> UpdateGenre(GenreRequestModel genre)
        {
            await _genreProvider.UpdateGenre(genre);

            return Ok();
        }

        [HttpGet, Route("get-top-genres")]
        public async Task<IActionResult> GetTopGenres()
        {
            IList<Genre> genres = await _genreProvider.GetTopGenres(TopGenresTake);

            return Json(genres);
        }

        [HttpGet, Route("get-top-movements")]
        public async Task<IActionResult> GetTopMovements()
        {
            IList<Genre> movements = await _genreProvider.GetTopMovements(TopGenresTake);

            return Json(movements);
        }

        [HttpGet, Route("get-top-genres-and-movements")]
        public async Task<IActionResult> GetTopGenresAndMovements()
        {
            IList<Genre> genres = await _genreProvider.GetTopGenresAndMovements(TopGenresTake);

            return Json(genres);
        }

        [HttpGet, Route("get-all-genres-and-movements")]
        public async Task<IActionResult> GetAllGenresAndMovements()
        {
            IList<Genre> genres = await _genreProvider.GetAllGenresAndMovements();

            return Json(genres);
        }

        [HttpGet, Route("get-genre/{genreId}/{userId}")]
        public async Task<IActionResult> GetGenre(int genreId, int userId)
        {
            if(genreId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(genreId));
            }

            Genre genre = await _genreProvider.GetGenre(genreId);

            if(genre != null)
            {
                genre.Paintings = await _paintingProvider.GetPaintingsOfGenre(genreId, userId);
            }

            return Json(genre);
        }
    }
}
