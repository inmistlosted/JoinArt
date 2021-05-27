using diplom.api.Models;
using diplom.api.Models.RequestModels;
using diplom.api.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Providers
{
    public interface IGenreProvider
    {
        Task<GenreResponseModel> AddGenre(GenreRequestModel genre);
        Task UpdateGenre(GenreRequestModel genre);
        Task<IList<Genre>> GetTopGenres(int take, bool withCache = false);
        Task<IList<Genre>> GetTopMovements(int take, bool withCache = false);
        Task<IList<Genre>> GetAllGenresAndMovements(bool withCache = false);
        Task<IList<Genre>> GetPaintingGenres(int paintingId, bool withCache = false);
        Task<Genre> GetGenre(int genreId, bool withCache = false);
        Task<IList<Genre>> GetTopGenresAndMovements(int take, bool withCache = false);
    }
}
