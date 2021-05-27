using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer
{
    public interface IGenreAdapter
    {
        Task<IDataReader> AddGenre(string title, string description, byte[] image, bool isMovement);
        Task UpdateGenre(int id, string title, string description, byte[] image, bool isMovement);
        Task<IDataReader> GetAllGenres();
        Task<IDataReader> GetAllMovements();
        Task<IDataReader> GetAllGenresAndMovements();
        Task<IDataReader> GetGenreLikesCount(int genreId);
        Task<IDataReader> GetPaintingGenres(int paintingId);
        Task<IDataReader> GetGenre(int genreId);
    }
}
