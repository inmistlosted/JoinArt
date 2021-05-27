using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer.Implementation
{
    public class AlbumAdapter : IAlbumAdapter
    {
        private ICommandAdapter _commandAdapter;

        public AlbumAdapter(ICommandAdapter commandAdapter)
        {
            this._commandAdapter = commandAdapter ?? throw new ArgumentNullException(nameof(commandAdapter));
        }

        public async Task<IDataReader> Create(string title, string description, int userId)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            if(userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            using (NpgsqlCommand sqlCommand = CreateCreateCommand(title, description, userId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task Update(int albumId, string title, string description)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            using (NpgsqlCommand sqlCommand = CreateUpdateCommand(albumId, title, description))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task Delete(int albumId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            using (NpgsqlCommand sqlCommand = CreateDeleteCommand(albumId))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetAlbumsOfUser(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetAlbumsOfUserCommand(userId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetAlbum(int albumId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetAlbumCommand(albumId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetAllAlbums()
        {
            using (NpgsqlCommand sqlCommand = CreateGetAllAlbumsCommand())
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetAlbumLikesCount(int albumId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetAlbumLikesCountCommand(albumId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetAlbumImage(int albumId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetAlbumImageCommand(albumId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> AddPaintingToAlbum(int albumId, int paintingId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            using (NpgsqlCommand sqlCommand = CreateAddPaintingToAlbumCommand(albumId, paintingId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> RemovePaintingFromAlbum(int albumId, int paintingId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            using (NpgsqlCommand sqlCommand = CreateRemovePaintingFromAlbumCommand(albumId, paintingId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> IsAlbumBelongsToUser(int albumId, int userId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            using (NpgsqlCommand sqlCommand = CreateIsAlbumBelongsToUserCommand(albumId, userId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        private NpgsqlCommand CreateCreateCommand(string title, string description, int userId)
        {
            string query = @"insert into albums (title, description, userId)
                             values (@title, @description, @userId)
                             returning albumId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("title", title);
            sqlCommand.Parameters.AddWithValue("description", description ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("userId", userId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateUpdateCommand(int albumId, string title, string description)
        {
            string query = @"update albums set title = @title, description = @description where albumId = @albumId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("title", title);
            sqlCommand.Parameters.AddWithValue("description", description ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("albumId", albumId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateDeleteCommand(int albumId)
        {
            string query = @"
                    delete from albumpaintings where albumId = @albumId;
                    delete from albums where albumId = @albumId;";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("albumId", albumId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetAlbumsOfUserCommand(int userId)
        {
            string query = @"select * from albums where userId = @userId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("userId", userId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetAlbumCommand(int albumId)
        {
            string query = @"select * from albums where albumId = @albumId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("albumId", albumId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetAllAlbumsCommand()
        {
            string query = @"select * from albums";

            var sqlCommand = new NpgsqlCommand(query);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetAlbumLikesCountCommand(int albumId)
        {
            string query = @"select count(likeId)
                             from likes
                             where paintingid in (
                                    select paintingId 
                                    from albumpaintings 
                                    where albumId = @albumId
                             )";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("albumId", albumId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetAlbumImageCommand(int albumId)
        {
            string query = @"select image 
                             from paintings 
                             where paintingid in (
                                    select paintingId 
                                    from albumpaintings 
                                    where albumId = @albumId
                             ) limit 1";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("albumId", albumId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateAddPaintingToAlbumCommand(int albumId, int paintingId)
        {
            string query = @"insert into albumpaintings (albumId, paintingId) select @albumId, @paintingId 
                             where not exists (
                                    select * 
                                    from albumpaintings 
                                    where albumId = @albumId and paintingId = @paintingId
                             ) returning albumId;";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("albumId", albumId);
            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateRemovePaintingFromAlbumCommand(int albumId, int paintingId)
        {
            string query = @"delete from albumpaintings where albumId = @albumId and paintingId = @paintingId;";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("albumId", albumId);
            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateIsAlbumBelongsToUserCommand(int albumId, int userId)
        {
            string query = @"select albumId from albums where albumId = @albumId and userId = @userId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("albumId", albumId);
            sqlCommand.Parameters.AddWithValue("userId", userId);

            return sqlCommand;
        }
    }
}
