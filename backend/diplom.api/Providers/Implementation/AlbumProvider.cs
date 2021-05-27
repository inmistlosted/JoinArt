using diplom.api.Classes;
using diplom.api.DataAccessLayer;
using diplom.api.Models;
using diplom.api.Models.RequestModels;
using diplom.api.Models.ResponseModels;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Providers.Implementation
{
    public class AlbumProvider : IAlbumProvider
    {
        private readonly IDataAccessAdapter _dataAccessAdapter;
        private readonly IMemoryCache _cache;

        public AlbumProvider(IDataAccessAdapter dataAccessAdapter, IMemoryCache cache)
        {
            this._dataAccessAdapter = dataAccessAdapter ?? throw new ArgumentNullException(nameof(dataAccessAdapter));
            this._cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<CreateAlbumResponseModel> Create(CreateAlbumRequestModel album)
        {
            if (album == null)
            {
                throw new ArgumentNullException(nameof(album));
            }

            bool isAdded = false;
            int albumId = 0;

            using (IDataReader reader = await this._dataAccessAdapter.AlbumAdapter.Create(album.Title, album.Description, album.UserId))
            {
                if (reader.Read())
                {
                    albumId = (int)reader["albumId"];
                    isAdded = albumId != 0;
                }
            }

            CreateAlbumResponseModel response = new CreateAlbumResponseModel
            {
                Status = isAdded,
                AlbumId = albumId,
                Message = isAdded ? string.Empty : "Unexpected error occurre",
            };

            return response;
        }

        public async Task Update(CreateAlbumRequestModel album)
        {
            if (album == null)
            {
                throw new ArgumentNullException(nameof(album));
            }

            await this._dataAccessAdapter.AlbumAdapter.Update(album.AlbumId, album.Title, album.Description);
        }

        public async Task Delete(int albumId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            await this._dataAccessAdapter.AlbumAdapter.Delete(albumId);
        }

        public async Task<Album> GetAlbum(int albumId, int userId, bool withCache = false)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            string cacheKey = $"get_album_{albumId}_{userId}";

            Album album = withCache ? Helper.GetFromCache<Album>(this._cache, cacheKey) : null;

            if(album == null)
            {
                using (IDataReader reader = await this._dataAccessAdapter.AlbumAdapter.GetAlbum(albumId))
                {
                    if (reader.Read())
                    {
                        album = ConvertReaderToAlbum(reader);
                    }
                }

                album.LikesCount = await GetAlbumLikesCount(albumId);

                if(userId != 0)
                {
                    album.BelongsToUser = await BelongsToUser(albumId, userId);
                }

                Helper.SetToCache(this._cache, cacheKey, album);
            }

            return album;
        }

        public async Task<IList<Album>> GetAllAlbums(bool withCache = false)
        {
            string cacheKey = $"get_albums";

            IList<Album> albums = withCache ? Helper.GetFromCache<IList<Album>>(this._cache, cacheKey) : null;

            if (albums == null)
            {
                albums = new List<Album>();

                using (IDataReader reader = await this._dataAccessAdapter.AlbumAdapter.GetAllAlbums())
                {
                    while (reader.Read())
                    {
                        albums.Add(ConvertReaderToAlbum(reader));
                    }
                }

                IDictionary<int, long> likes = await GetAlbumsLikes(albums);

                foreach(Album album in albums)
                {
                    album.Image = await GetAlbumImage(album.AlbumId);
                    album.LikesCount = likes.First(x => x.Key == album.AlbumId).Value;
                }

                Helper.SetToCache(this._cache, cacheKey, albums);
            }

            return albums;
        }

        public async Task<IList<Album>> GetTopAlbums(int take, bool withCache = false)
        {
            if (take <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(take));
            }

            string cacheKey = "get_top_albums";

            IList<Album> topAlbums = withCache ? Helper.GetFromCache<IList<Album>>(this._cache, cacheKey) : null;

            if (topAlbums == null)
            {
                IList<Album> allAlbums = await GetAllAlbums();

                if (allAlbums != null)
                {
                    topAlbums = allAlbums.Take(take).OrderByDescending(x => x.LikesCount).ToList();

                    Helper.SetToCache(this._cache, cacheKey, topAlbums);
                }
            }

            return topAlbums;
        }

        public async Task<IList<Album>> GetUserAlbums(int userId, bool withCache = false)
        {
            if(userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            string cacheKey = $"get_albums_of_user_{userId}";

            IList<Album> albums = withCache ? Helper.GetFromCache<IList<Album>>(this._cache, cacheKey) : null;

            if (albums == null)
            {
                albums = new List<Album>();

                using (IDataReader reader = await this._dataAccessAdapter.AlbumAdapter.GetAlbumsOfUser(userId))
                {
                    while (reader.Read())
                    {
                        albums.Add(ConvertReaderToAlbum(reader));
                    }
                }

                foreach (Album album in albums)
                {
                    album.Image = await GetAlbumImage(album.AlbumId);
                }

                Helper.SetToCache(this._cache, cacheKey, albums);
            }

            return albums;
        }

        public async Task<AddPaintingToAlbumModel> AddPaintingToAlbum(int albumId, int paintingId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            bool isAdded = false;

            using (IDataReader reader = await this._dataAccessAdapter.AlbumAdapter.AddPaintingToAlbum(albumId, paintingId))
            {
                if (reader.Read())
                {
                    isAdded = (int)reader["albumId"] != 0;
                }
            }

            AddPaintingToAlbumModel response = new AddPaintingToAlbumModel
            {
                IsAdded = isAdded,
                Message = isAdded ? string.Empty : "Painting already exists in this album",
            };

            return response;
        }

        public async Task RemovePaintingFromAlbum(int albumId, int paintingId)
        {
            if (albumId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(albumId));
            }

            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            await this._dataAccessAdapter.AlbumAdapter.RemovePaintingFromAlbum(albumId, paintingId);
        }

        private async Task<IDictionary<int, long>> GetAlbumsLikes(IList<Album> albums)
        {
            IDictionary<int, long> albumsLikes = new Dictionary<int, long>();

            foreach (Album album in albums)
            {
                using (IDataReader reader = await this._dataAccessAdapter.AlbumAdapter.GetAlbumLikesCount(album.AlbumId))
                {
                    if (reader.Read())
                    {
                        long likesCount = (long)reader["count"];

                        albumsLikes.Add(album.AlbumId, likesCount);
                    }
                }
            }

            return albumsLikes;
        }

        private async Task<long> GetAlbumLikesCount(int albumId)
        {
            using (IDataReader reader = await this._dataAccessAdapter.AlbumAdapter.GetAlbumLikesCount(albumId))
            {
                if (reader.Read())
                {
                    return (long)reader["count"];
                }
            }

            return 0;
        }

        private async Task<string> GetAlbumImage(int albumId)
        {
            using (IDataReader reader = await this._dataAccessAdapter.AlbumAdapter.GetAlbumImage(albumId))
            {
                if (reader.Read())
                {
                    return Helper.ToRenderablePictureString(reader["Image"] == DBNull.Value ? null : (byte[])reader["Image"]);
                }
            }

            return string.Empty;
        }

        private async Task<bool> BelongsToUser(int albumId, int userId)
        {
            bool belongs = false;

            using (IDataReader reader = await this._dataAccessAdapter.AlbumAdapter.IsAlbumBelongsToUser(albumId, userId))
            {
                if (reader.Read())
                {
                    belongs = (int)reader["albumId"] != 0;
                }
            }

            return belongs;
        }

        private Album ConvertReaderToAlbum(IDataReader reader)
        {
            return new Album
            {
                AlbumId = (int)reader["albumId"],
                Title = reader["title"].ToString(),
                Description = reader["description"] == DBNull.Value ? null : reader["description"].ToString(),
                UserId = (int)reader["userId"],
            };
        }
    }
}
