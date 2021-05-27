using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer.Implementation
{
    public class PaintingAdapter : IPaintingAdapter
    {
        private ICommandAdapter _commandAdapter;

        public PaintingAdapter(ICommandAdapter commandAdapter)
        {
            this._commandAdapter = commandAdapter ?? throw new ArgumentNullException(nameof(commandAdapter));
        }

        public async Task<IDataReader> GetAll()
        {
            string sqlQuery = "select * from paintings";

            using (NpgsqlCommand sqlCommand = new NpgsqlCommand(sqlQuery))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetPainting(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetPaintingCommand(paintingId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetPaintingComments(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetPaintingCommentsCommand(paintingId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetPaintingCommentsCount(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetPaintingCommentsCountCommand(paintingId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetPaintingLikesCount(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetPaintingLikesCountCommand(paintingId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> IsPaintingLiked(int paintingId, int userId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            using (NpgsqlCommand sqlCommand = CreateIsPaintingLikedCommand(paintingId, userId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> IsBidProduct(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            using (NpgsqlCommand sqlCommand = CreateIsBidProductCommand(paintingId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> AddPainting(string title, string materials, string painter, double price, string description, DateTime uploadDate, bool status, byte[] image)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (string.IsNullOrEmpty(painter))
            {
                throw new ArgumentNullException(nameof(painter));
            }

            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price));
            }

            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            using (NpgsqlCommand sqlCommand = CreateAddPaintingCommand(title, materials, painter, price, description, uploadDate, status, image))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> UpdatePainting(int paintingId, string title, string materials, string painter, double price, string description, bool status, byte[] image)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException(nameof(title));
            }

            if (string.IsNullOrEmpty(painter))
            {
                throw new ArgumentNullException(nameof(painter));
            }

            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price));
            }

            using (NpgsqlCommand sqlCommand = CreateUpdatePaintingCommand(paintingId, title, materials, painter, price, description, status, image))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task AddPaintingOwner(int paintingId, int ownerId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (ownerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ownerId));
            }

            using (NpgsqlCommand sqlCommand = CreateAddPaintingOwnerCommand(paintingId, ownerId))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task AddPaintingGenres(int paintingId, IList<int> genresIds)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (genresIds == null || !genresIds.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(genresIds));
            }

            using (NpgsqlCommand sqlCommand = CreateAddPaintingGenresCommand(paintingId, genresIds))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task RemovePaintingGenres(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            using (NpgsqlCommand sqlCommand = CreateRemovePaintingGenresCommand(paintingId))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> AddLike(int paintingId, int userId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            using (NpgsqlCommand sqlCommand = CreateAddLikeCommand(paintingId, userId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task RemoveLike(int paintingId, int userId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            using (NpgsqlCommand sqlCommand = CreateRemoveLikeCommand(paintingId, userId))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> AddComment(int paintingId, int userId, string content, DateTime date)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentOutOfRangeException(nameof(content));
            }

            using (NpgsqlCommand sqlCommand = CreateAddCommentCommand(paintingId, userId, content, date))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetPaintingsOfGenre(int genreId)
        {
            if (genreId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(genreId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetPaintingsOfGenreCommand(genreId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetPaintingsOfAlbum(int albumId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetPaintingsOfAlbumCommand(albumId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetPaintingsOfUser(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetPaintingsOfUserCommand(userId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> IsPaintingInAlbum(int paintingId, int albumId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            using (NpgsqlCommand sqlCommand = CreateIsPaintingInAlbumCommand(paintingId, albumId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> SearchPaintings(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(nameof(query));
            }

            using (NpgsqlCommand sqlCommand = CreateSearchPaintingsCommand(query))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetPaintingPrice(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetPaintingPriceCommand(paintingId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task UpdatePaintingPrice(int paintingId, double price)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price));
            }

            using (NpgsqlCommand sqlCommand = CreateUpdatePaintingPriceCommand(paintingId, price))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task DeletePainting(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            using (NpgsqlCommand sqlCommand = CreateDeletePaintingCommand(paintingId))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        private NpgsqlCommand CreateGetPaintingCommand(int paintingId)
        {
            string query = @"select * from paintings where paintingId = @paintingId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetPaintingCommentsCommand(int paintingId)
        {
            string query = @"select comments.commentId, comments.content, comments.date, users.login
                             from comments inner join users on comments.userId = users.userId
                             where comments.paintingId = @paintingId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetPaintingCommentsCountCommand(int paintingId)
        {
            string query = @"select count(commentId)
                             from comments
                             where paintingId = @paintingId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetPaintingLikesCountCommand(int paintingId)
        {
            string query = @"select count(likeId)
                             from likes
                             where paintingId = @paintingId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateIsPaintingLikedCommand(int paintingId, int userId)
        {
            string query = @"select likeId
                             from likes
                             where paintingId = @paintingId and userId = @userId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);
            sqlCommand.Parameters.AddWithValue("userId", userId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateIsBidProductCommand(int paintingId)
        {
            string query = @"select bidid from bidproducts where paintingid = @paintingId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateAddPaintingCommand(string title, string materials, string painter, double price, string description, DateTime uploadDate, bool status, byte[] image)
        {
            string query = @"insert into paintings (title, materials, painter, price, description, uploadDate, status, image)
                             values (@title, @materials, @painter, @price, @description, @uploadDate, @status, @image)
                             returning paintingId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("title", title);
            sqlCommand.Parameters.AddWithValue("materials", materials ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("painter", painter);
            sqlCommand.Parameters.AddWithValue("price", price);
            sqlCommand.Parameters.AddWithValue("description", description ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("uploadDate", uploadDate);
            sqlCommand.Parameters.AddWithValue("image", image ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("status", status);

            return sqlCommand;
        }

        private NpgsqlCommand CreateUpdatePaintingCommand(int paintingId, string title, string materials, string painter, double price, string description, bool status, byte[] image)
        {
            string imageUpdate = image == null ? string.Empty : ", image = @image";

            string query = @$"update paintings 
                             set title = @title, materials = @materials, painter = @painter, price = @price, description = @description, status = @status {imageUpdate}
                             where paintingId = @paintingId
                             returning paintingId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);
            sqlCommand.Parameters.AddWithValue("title", title);
            sqlCommand.Parameters.AddWithValue("materials", materials ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("painter", painter);
            sqlCommand.Parameters.AddWithValue("price", price);
            sqlCommand.Parameters.AddWithValue("description", description ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("image", image ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("status", status);

            return sqlCommand;
        }

        private NpgsqlCommand CreateAddPaintingOwnerCommand(int paintingId, int ownerId)
        {
            string query = @"insert into paintingsusers (paintingId, userId) values (@paintingId, @userId);";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);
            sqlCommand.Parameters.AddWithValue("userId", ownerId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateAddPaintingGenresCommand(int paintingId, IList<int> genresIds)
        {
            string query = string.Empty;

            foreach(int genreId in genresIds)
            {
                query += @$"insert into paintinggenres (paintingId, genreId) values ({paintingId}, {genreId});";
            }

            var sqlCommand = new NpgsqlCommand(query);

            return sqlCommand;
        }

        private NpgsqlCommand CreateRemovePaintingGenresCommand(int paintingId)
        {
            string query = @"delete from paintinggenres where paintingId = @paintingId;";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateAddLikeCommand(int paintingId, int userId)
        {
            string query = @"insert into likes (userId, paintingId) values (@userId, @paintingId) returning likeId;";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);
            sqlCommand.Parameters.AddWithValue("userId", userId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateRemoveLikeCommand(int paintingId, int userId)
        {
            string query = @"delete from likes where userId = @userId and paintingId = @paintingId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);
            sqlCommand.Parameters.AddWithValue("userId", userId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateAddCommentCommand(int paintingId, int userId, string content, DateTime date)
        {
            string query = @"insert into comments (content, date, userId, paintingId) 
                             values (@content, @date, @userId, @paintingId) returning commentId;";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("content", content);
            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);
            sqlCommand.Parameters.AddWithValue("userId", userId);
            sqlCommand.Parameters.AddWithValue("date", date);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetPaintingsOfGenreCommand(int genreId)
        {
            string query = @"select * 
                             from paintings 
                             where paintingid in (
                                    select paintingId 
                                    from paintinggenres 
                                    where genreId = @genreId)";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("genreId", genreId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetPaintingsOfAlbumCommand(int albumId)
        {
            string query = @"select * 
                             from paintings 
                             where paintingid in (
                                    select paintingId 
                                    from albumpaintings 
                                    where albumId = @albumId)";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("albumId", albumId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetPaintingsOfUserCommand(int userId)
        {
            string query = @"select * 
                             from paintings 
                             where paintingid in (
                                    select paintingId 
                                    from paintingsusers 
                                    where userId = @userId)";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("userId", userId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateIsPaintingInAlbumCommand(int paintingId, int albumId)
        {
            string query = @"select paintingId
                             from albumPaintings
                             where paintingId = @paintingId and albumId = @albumId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);
            sqlCommand.Parameters.AddWithValue("albumId", albumId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateSearchPaintingsCommand(string query)
        {
            string queryString = @$"select * from paintings where lower(title) like '%{query}%'";

            var sqlCommand = new NpgsqlCommand(queryString);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetPaintingPriceCommand(int paintingId)
        {
            string query = @"select price from paintings where paintingId = @paintingId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateUpdatePaintingPriceCommand(int paintingId, double price)
        {
            string query = @"update paintings set price = @price where paintingId = @paintingId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("price", price);
            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateDeletePaintingCommand(int paintingId)
        {
            string query = @"
                delete from albumpaintings where paintingId = @paintingId;
                delete from bidbets where bidid = (select bidid from bidproducts where paintingId = @paintingId);
                delete from bidproducts where paintingId = @paintingId;
                delete from comments where paintingId = @paintingId;
                delete from likes where paintingId = @paintingId;
                delete from paintinggenres where paintingId = @paintingId;
                delete from paintingsusers where paintingId = @paintingId;
                delete from paintings where paintingId = @paintingId;";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId", paintingId);

            return sqlCommand;
        }
    }
}
