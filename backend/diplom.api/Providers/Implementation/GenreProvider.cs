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
    public class GenreProvider : IGenreProvider
    {
        private readonly IDataAccessAdapter _dataAccessAdapter;
        private readonly IMemoryCache _cache;

        public GenreProvider(IDataAccessAdapter dataAccessAdapter, IMemoryCache cache)
        {
            this._dataAccessAdapter = dataAccessAdapter ?? throw new ArgumentNullException(nameof(dataAccessAdapter));
            this._cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<GenreResponseModel> AddGenre(GenreRequestModel genre)
        {
            if(genre == null)
            {
                throw new ArgumentNullException(nameof(genre));
            }

            byte[] image = Helper.FileAsBytes(genre.Image);
            bool isAdded = false;

            using (IDataReader reader = await this._dataAccessAdapter.GenreAdapter.AddGenre(genre.Title, genre.Description, image, genre.IsMovement))
            {
                if (reader.Read())
                {
                    isAdded = (int)reader["genreId"] != 0;
                }
            }

            GenreResponseModel response = new GenreResponseModel
            {
                Status = isAdded,
                Message = isAdded ? string.Empty : "Unexpected error occurred"
            };

            return response;
        }

        public async Task UpdateGenre(GenreRequestModel genre)
        {
            if (genre == null)
            {
                throw new ArgumentNullException(nameof(genre));
            }

            byte[] image = Helper.FileAsBytes(genre.Image);

            await this._dataAccessAdapter.GenreAdapter.UpdateGenre(genre.GenreId, genre.Title, genre.Description, image, genre.IsMovement);
        }

        public async Task<IList<Genre>> GetTopGenres(int take, bool withCache = false)
        {
            if (take <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(take));
            }

            string cacheKey = "get_top_genres";

            IList<Genre> topGenres = withCache ? Helper.GetFromCache<IList<Genre>>(this._cache, cacheKey) : null;

            if(topGenres == null)
            {
                IList<Genre> genres = await GetAllGenres();

                if(genres != null)
                {
                    IDictionary<int, long> genreLikes = await GetGenresLikes(genres);

                    foreach(Genre genre in genres)
                    {
                        genre.LikesCount = genreLikes.ContainsKey(genre.GenreId) ? genreLikes[genre.GenreId] : 0;
                    }

                    topGenres = genres.OrderByDescending(x => x.LikesCount).Take(take).ToList();

                    Helper.SetToCache(this._cache, cacheKey, topGenres);
                }
            }

            return topGenres;
        }

        public async Task<IList<Genre>> GetTopMovements(int take, bool withCache = false)
        {
            if (take <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(take));
            }

            string cacheKey = "get_top_movements";

            IList<Genre> topMovements = withCache ? Helper.GetFromCache<IList<Genre>>(this._cache, cacheKey) : null;

            if(topMovements == null)
            {
                IList<Genre> movements = await GetAllMovements();

                if(movements != null)
                {
                    IDictionary<int, long> movementsLikes = await GetGenresLikes(movements);

                    foreach(Genre movement in movements)
                    {
                        movement.LikesCount = movementsLikes.ContainsKey(movement.GenreId) ? movementsLikes[movement.GenreId] : 0;
                    }

                    topMovements = movements.OrderByDescending(x => x.LikesCount).Take(take).ToList();

                    Helper.SetToCache(this._cache, cacheKey, topMovements);
                }
            }

            return topMovements;
        }

        public async Task<IList<Genre>> GetAllGenresAndMovements(bool withCache = false)
        {
            string cacheKey = "get_all_genres_and_movements";

            IList<Genre> genresAndMovements = withCache ? Helper.GetFromCache<IList<Genre>>(this._cache, cacheKey) : null;

            if(genresAndMovements == null)
            {
                genresAndMovements = new List<Genre>();

                using (IDataReader reader = await this._dataAccessAdapter.GenreAdapter.GetAllGenresAndMovements())
                {
                    while (reader.Read())
                    {
                        genresAndMovements.Add(ConvertReaderToGenre(reader));
                    }
                }

                IDictionary<int, long> genresLikesCount = await GetGenresLikes(genresAndMovements);

                foreach(Genre genre in genresAndMovements)
                {
                    genre.LikesCount = genresLikesCount.ContainsKey(genre.GenreId) ? genresLikesCount[genre.GenreId] : 0;
                }

                Helper.SetToCache(this._cache, cacheKey, genresAndMovements);
            }

            return genresAndMovements;
        }

        public async Task<IList<Genre>> GetTopGenresAndMovements(int take, bool withCache = false)
        {
            if (take <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(take));
            }

            string cacheKey = "get_top_genres_and_movements";

            IList<Genre> topGenres = withCache ? Helper.GetFromCache<IList<Genre>>(this._cache, cacheKey) : null;

            if (topGenres == null)
            {
                IList<Genre> genresAndMovements = await GetAllGenresAndMovements();

                if (genresAndMovements != null)
                {
                    IDictionary<int, long> genreLikes = await GetGenresLikes(genresAndMovements);

                    foreach (Genre genre in genresAndMovements)
                    {
                        genre.LikesCount = genreLikes.ContainsKey(genre.GenreId) ? genreLikes[genre.GenreId] : 0;
                    }

                    topGenres = genresAndMovements.OrderByDescending(x => x.LikesCount).Take(take).ToList();

                    Helper.SetToCache(this._cache, cacheKey, topGenres);
                }
            }

            return topGenres;
        }

        public async Task<IList<Genre>> GetPaintingGenres(int paintingId, bool withCache = false)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            string cacheKey = "get_top_genres";

            IList<Genre> genres = withCache ? Helper.GetFromCache<IList<Genre>>(this._cache, cacheKey) : null;

            if (genres == null)
            {
                genres = new List<Genre>();

                using (IDataReader reader = await this._dataAccessAdapter.GenreAdapter.GetPaintingGenres(paintingId))
                {
                    while (reader.Read())
                    {
                        genres.Add(ConvertReaderToGenre(reader));
                    }
                }
            }

            return genres;
        }

        public async Task<Genre> GetGenre(int genreId, bool withCache = false)
        {
            if(genreId <= 0)
            {
                throw new ArgumentNullException(nameof(genreId));
            }

            string cacheKey = $"get_genre_{genreId}";

            Genre genre = withCache ? Helper.GetFromCache<Genre>(this._cache, cacheKey) : null;

            if (genre == null)
            {
                using (IDataReader reader = await this._dataAccessAdapter.GenreAdapter.GetGenre(genreId))
                {
                    while (reader.Read())
                    {
                        genre = ConvertReaderToGenre(reader);
                    }
                }

                Helper.SetToCache(this._cache, cacheKey, genre);
            }

            return genre;
        }

        private async Task<IDictionary<int, long>> GetGenresLikes(IList<Genre> genres)
        {
            IDictionary<int, long> genresLikes = new Dictionary<int, long>();

            foreach(Genre genre in genres)
            {
                using (IDataReader reader = await this._dataAccessAdapter.GenreAdapter.GetGenreLikesCount(genre.GenreId))
                {
                    if(reader.Read())
                    {
                        long likesCount = (long)reader["count"];

                        genresLikes.Add(genre.GenreId, likesCount);
                    }
                }
            }

            return genresLikes;
        }

        private async Task<IList<Genre>> GetAllGenres()
        {
            IList<Genre> genres = new List<Genre>();

            using (IDataReader reader = await this._dataAccessAdapter.GenreAdapter.GetAllGenres())
            {
                while (reader.Read())
                {
                    genres.Add(ConvertReaderToGenre(reader));
                }
            }

            return genres;
        }

        private async Task<IList<Genre>> GetAllMovements()
        {
            IList<Genre> genres = new List<Genre>();

            using (IDataReader reader = await this._dataAccessAdapter.GenreAdapter.GetAllMovements())
            {
                while (reader.Read())
                {
                    genres.Add(ConvertReaderToGenre(reader));
                }
            }

            return genres;
        }

        private Genre ConvertReaderToGenre(IDataReader reader)
        {
            byte[] image = reader["image"] == DBNull.Value ? null : (byte[])reader["image"];

            return new Genre
            {
                GenreId = (int)reader["genreId"],
                Title = reader["title"].ToString(),
                Description = reader["description"] == DBNull.Value ? null : reader["description"].ToString(),
                Image = Helper.ToRenderablePictureString(image),
                IsMovement = (bool)reader["isMovement"],
            };
        }
    }
}
