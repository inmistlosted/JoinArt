using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer.Implementation
{
    public class GenreAdapter : IGenreAdapter
    {
        private ICommandAdapter _commandAdapter;

        public GenreAdapter(ICommandAdapter commandAdapter)
        {
            this._commandAdapter = commandAdapter ?? throw new ArgumentNullException(nameof(commandAdapter));
        }

        public async Task<IDataReader> AddGenre(string title, string description, byte[] image, bool isMovement)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            using (NpgsqlCommand sqlCommand = CreateAddGenreCommand(title, description, image, isMovement))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task UpdateGenre(int genreId, string title, string description, byte[] image, bool isMovement)
        {
            if(genreId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(genreId));
            }

            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            using (NpgsqlCommand sqlCommand = CreateUpdateCommand(genreId, title, description, image, isMovement))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetAllGenres()
        {
            using (NpgsqlCommand sqlCommand = CreateGetAllGenresCommand())
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetAllMovements()
        {
            using (NpgsqlCommand sqlCommand = CreateGetAllMovementsCommand())
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetAllGenresAndMovements()
        {
            using (NpgsqlCommand sqlCommand = CreateGetAllGenresAndMovementsCommand())
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetGenreLikesCount(int genreId)
        {
            if (genreId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(genreId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetGenreLikesCountCommand(genreId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetPaintingGenres(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetPaintingGenresCommand(paintingId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetGenre(int genreId)
        {
            if (genreId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(genreId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetGenreCommand(genreId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        private NpgsqlCommand CreateAddGenreCommand(string title, string description, byte[] image, bool isMovement)
        {
            string query = @"insert into genres (title, description, image, isMovement)
                             values (@title, @description, @image, @isMovement)
                             returning genreId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("title", title);
            sqlCommand.Parameters.AddWithValue("description", description ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("image", image ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("isMovement", isMovement);

            return sqlCommand;
        }

        private NpgsqlCommand CreateUpdateCommand(int genreId, string title, string description, byte[] image, bool isMovement)
        {
            string query = @"update genres 
                             set title = @title, description = @description, image = @image, isMovement = @isMovement)
                             where genreId = @genreId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("genreId", genreId);
            sqlCommand.Parameters.AddWithValue("title", title);
            sqlCommand.Parameters.AddWithValue("description", description ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("image", image ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("isMovement", isMovement);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetAllGenresCommand()
        {
            string query = @"select * from genres where isMovement = false";

            var sqlCommand = new NpgsqlCommand(query);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetAllMovementsCommand()
        {
            string query = @"select * from genres where isMovement = true";

            var sqlCommand = new NpgsqlCommand(query);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetAllGenresAndMovementsCommand()
        {
            string query = @"select * from genres";

            var sqlCommand = new NpgsqlCommand(query);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetGenreLikesCountCommand(int genreId)
        {
            string query = @"select count(likeId)
                             from likes
                             where paintingid in (
                                    select paintingId 
                                    from paintinggenres 
                                    where genreId = @genreId
                             )";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("genreId", genreId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetPaintingGenresCommand(int paintingId)
        {
            string query = @"select * 
                             from paintinggenres inner join genres on paintinggenres.genreId = genres.genreid 
                             where paintingid = @paintingId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetGenreCommand(int genreId)
        {
            string query = @"select * from genres where genreId = @genreId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("genreId", genreId);

            return sqlCommand;
        }
    }
}
