using diplom.api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.IO;

namespace diplom.api.Classes
{
    public class Helper
    {
        public const double ViewsFactor = 0.7;
        public const double LikesFactor = 0.5;
        public const double CommentsFactor = 0.3;

        public static T GetFromCache<T>(IMemoryCache memoryCache, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (memoryCache == null)
            {
                throw new ArgumentNullException(nameof(memoryCache));
            }

            memoryCache.TryGetValue(key, out object cachedItem);

            if (typeof(T) == typeof(string) && cachedItem is long)
            {
                object obj = Convert.ToString(cachedItem);

                return (T)obj;
            }

            if (typeof(T) == typeof(long) && cachedItem is string)
            {
                object obj = Convert.ToInt64(cachedItem);

                return (T)obj;
            }

            if (cachedItem == null)
            {
                return memoryCache.Get<T>(key);
            }

            return (T)cachedItem;
        }

        public static void SetToCache(IMemoryCache memoryCache, string key, object obj, int minutes = 60)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (memoryCache == null)
            {
                throw new ArgumentNullException(nameof(memoryCache));
            }

            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            memoryCache.Set(key, obj, TimeSpan.FromMinutes(minutes));
        }

        public static byte[] FileAsBytes(IFormFile file)
        {
            if (file == null)
            {
                return null;
            }

            byte[] fileAsBytes;

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                fileAsBytes = memoryStream.ToArray();
            };

            return fileAsBytes;
        }

        public static IFormFile BytesToFile(byte[] fileBytes)
        {
            if (fileBytes == null)
            {
                return null;
            }

            IFormFile file = null;

            using (var memoryStream = new MemoryStream(fileBytes))
            {
                file = new FormFile(memoryStream, 0, fileBytes.Length, "name", "fileName");
            };

            return file;
        }

        public static string ToRenderablePictureString(byte[] array)
        {
            if (array == null)
            {
                return string.Empty;
            }

            return "data:image; base64," + Convert.ToBase64String(array);
        }

        public static IList<int> GetListFromString(string str)
        {
            IList<int> list = new List<int>();

            if (string.IsNullOrEmpty(str))
            {
                return list;
            }

            string[] nums = str.Split(",");

            foreach(string number in nums)
            {
                if (int.TryParse(number, out int item))
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public static double CalculatePaintingRating(Painting painting)
        {
            double rating = painting.BetsCount + ViewsFactor * painting.ViewsCount + LikesFactor * painting.LikesCount + CommentsFactor * painting.CommentsCount;

            return rating;
        }
    }
}
