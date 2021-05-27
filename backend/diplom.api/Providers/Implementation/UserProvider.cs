using diplom.api.Classes;
using diplom.api.DataAccessLayer;
using diplom.api.Models;
using diplom.api.Models.ResponseModels;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace diplom.api.Providers.Implementation
{
    public class UserProvider : IUserProvider
    {
        private readonly IDataAccessAdapter _dataAccessAdapter;
        private readonly IMemoryCache _cache;

        public UserProvider(IDataAccessAdapter dataAccessAdapter, IMemoryCache cache)
        {
            this._dataAccessAdapter = dataAccessAdapter ?? throw new ArgumentNullException(nameof(dataAccessAdapter));
            this._cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<RegisterResponseModel> AddUser(RegisterModel registerModel)
        {
            if(registerModel == null)
            {
                throw new ArgumentNullException(nameof(registerModel));
            }

            bool isAdded = false;
            int userId = 0;
            bool userExists = await UserExists(registerModel.Login, registerModel.Email);

            if (!userExists)
            {
                string passwordHash = EncryptPassword(registerModel.Password);

                using (IDataReader reader = await this._dataAccessAdapter.UserAdapter.AddUser(
                    registerModel.FirstName,
                    registerModel.SecondName,
                    registerModel.Address,
                    registerModel.Email,
                    registerModel.Phone,
                    registerModel.Country,
                    registerModel.Birthday,
                    registerModel.Gender,
                    registerModel.Login,
                    passwordHash))
                {
                    if (reader.Read())
                    {
                        userId = (int)reader["userId"];
                        isAdded = userId != 0;
                    }
                }
            }

            RegisterResponseModel response = new RegisterResponseModel {
                Status = isAdded,
            };

            if (isAdded)
            {
                int roleId = await GetUserRole(userId);

                response.UserId = userId;
                response.Login = registerModel.Login;
                response.RoleId = roleId;
            }
            else
            {
                response.Message = $"User with login {registerModel.Login} or email {registerModel.Email} already exists";
            }

            return response;
        }

        public async Task<RegisterResponseModel> UpdateUser(RegisterModel registerModel)
        {
            if (registerModel == null)
            {
                throw new ArgumentNullException(nameof(registerModel));
            }

            bool userExists = await UserExists(registerModel.Login, registerModel.Email);

            if (!userExists)
            {
                await this._dataAccessAdapter.UserAdapter.UpdateUser(
                    registerModel.UserId,
                    registerModel.FirstName,
                    registerModel.SecondName,
                    registerModel.Address,
                    registerModel.Email,
                    registerModel.Phone,
                    registerModel.Country,
                    registerModel.Birthday,
                    registerModel.Gender,
                    registerModel.Login);
            }

            RegisterResponseModel response = new RegisterResponseModel
            {
                Status = !userExists,
                Message = !userExists ? $"User with login {registerModel.Login} or email {registerModel.Email} already exists" : string.Empty,
            };

            return response;
        }

        public async Task<LoginResponseModel> LoginUser(LoginModel loginModel)
        {
            if (loginModel == null)
            {
                throw new ArgumentNullException(nameof(loginModel));
            }

            string passwordHash = await GetPasswordHash(loginModel.Login);
            bool isAuthorized = false;

            if (!string.IsNullOrEmpty(passwordHash) && VerifyPassword(loginModel.Password, passwordHash))
            {
                isAuthorized = true;
            }

            LoginResponseModel response = new LoginResponseModel
            {
                Status = isAuthorized,
            };

            if (isAuthorized)
            {
                int roleId = await GetUserRole(loginModel.Login);
                int userId = await GetUserId(loginModel.Login);

                response.UserId = userId;
                response.Login = loginModel.Login;
                response.RoleId = roleId;
            }
            else
            {
                response.Message = $"Incorrect login or password";
            }

            return response;
        }

        public async Task<User> GetPaintingOwner(int paintingId, bool withCache = false)
        {
            if(paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            string cacheKey = $"get_painting_owner_{paintingId}";

            User owner = withCache ? Helper.GetFromCache<User>(this._cache, cacheKey) : null;

            if(owner == null)
            {
                using (IDataReader reader = await this._dataAccessAdapter.UserAdapter.GetPaintingOwner(paintingId))
                {
                    if (reader.Read())
                    {
                        owner = ConvertReaderToUser(reader);
                    }
                }

                if(owner != null)
                {
                    Helper.SetToCache(this._cache, cacheKey, owner);
                }
            }

            return owner;
        }

        public async Task<bool> IsFollowing(int userId1, int userId2)
        {
            if (userId1 <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId1));
            }

            if (userId2 <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId2));
            }

            bool isFollowing = false;

            using (IDataReader reader = await this._dataAccessAdapter.UserAdapter.IsFollowingPaintingOwner(userId1, userId2))
            {
                if (reader.Read())
                {
                    isFollowing = (int)reader["followerId"] != 0;
                }
            }

            return isFollowing;
        }

        public async Task FollowUser(int userId1, int userId2)
        {
            if (userId1 <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId1));
            }

            if (userId2 <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId2));
            }

            await this._dataAccessAdapter.UserAdapter.FollowUser(userId1, userId2);
        }

        public async Task UnfollowUser(int userId1, int userId2)
        {
            if (userId1 <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId1));
            }

            if (userId2 <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId2));
            }

            await this._dataAccessAdapter.UserAdapter.UnfollowUser(userId1, userId2);
        }

        public async Task<User> GetUser(int userId, bool withCache = false)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            string cacheKey = $"get_user_{userId}";

            User user = withCache ? Helper.GetFromCache<User>(this._cache, cacheKey) : null;

            if (user == null)
            {
                using (IDataReader reader = await this._dataAccessAdapter.UserAdapter.GetUser(userId))
                {
                    if (reader.Read())
                    {
                        user = ConvertReaderToUser(reader);
                    }
                }

                if (user != null)
                {
                    user.Followers = await GetUserFollowers(userId);
                    user.Followings = await GetUserFollowing(userId);

                    Helper.SetToCache(this._cache, cacheKey, user);
                }
            }

            return user;
        }

        private async Task<IList<User>> GetUserFollowers(int userId)
        {
            IList<User> followers = new List<User>();

            using (IDataReader reader = await this._dataAccessAdapter.UserAdapter.GetUserFollowers(userId))
            {
                while (reader.Read())
                {
                    followers.Add(ConvertReaderToUser(reader));
                }
            }

            foreach(User follower in followers)
            {
                follower.IsFollowing = await IsFollowing(userId, follower.UserId);
            }

            return followers;
        }

        private async Task<IList<User>> GetUserFollowing(int userId)
        {
            IList<User> followings = new List<User>();

            using (IDataReader reader = await this._dataAccessAdapter.UserAdapter.GetUserFollowing(userId))
            {
                while (reader.Read())
                {
                    followings.Add(ConvertReaderToUser(reader));
                }
            }

            foreach(User following in followings)
            {
                following.IsFollowing = true;
            }

            return followings;
        }

        private async Task<int> GetUserRole(int userId)
        {
            using (IDataReader reader = await this._dataAccessAdapter.UserAdapter.GetUserRole(userId))
            {
                if (reader.Read())
                {
                    return (int)reader["roleId"];
                }
            }

            return 0;
        }

        private async Task<int> GetUserRole(string login)
        {
            using (IDataReader reader = await this._dataAccessAdapter.UserAdapter.GetUserRole(login))
            {
                if (reader.Read())
                {
                    return (int)reader["roleId"];
                }
            }

            return 0;
        }

        private async Task<int> GetUserId(string login)
        {
            using (IDataReader reader = await this._dataAccessAdapter.UserAdapter.GetUserId(login))
            {
                if (reader.Read())
                {
                    return (int)reader["userId"];
                }
            }

            return 0;
        }

        private async Task<bool> UserExists(string login, string email)
        {
            using (IDataReader reader = await this._dataAccessAdapter.UserAdapter.UserExists(login, email))
            {
                if (reader.Read())
                {
                    return (int)reader["userId"] != 0;
                }
            }

            return false;
        }

        private async Task<string> GetPasswordHash(string login)
        {
            using (IDataReader reader = await this._dataAccessAdapter.UserAdapter.GetUserPasswordByLogin(login))
            {
                if (reader.Read())
                {
                    return reader["password"].ToString();
                }
            }

            return string.Empty;
        }

        private string EncryptPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string passwordHash = GetHash(sha256Hash, password);

                return passwordHash;
            }
        }

        private bool VerifyPassword(string inputPassword, string passwordHash)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                string hashOfInputPassword = GetHash(sha256Hash, inputPassword);

                StringComparer comparer = StringComparer.OrdinalIgnoreCase;

                return comparer.Compare(hashOfInputPassword, passwordHash) == 0;
            }
        }

        private string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        private User ConvertReaderToUser(IDataReader reader)
        {
            return new User
            {
                UserId = (int)reader["userId"],
                FirstName = reader["firstName"].ToString(),
                SecondName = reader["lastName"].ToString(),
                Address = reader["address"] == DBNull.Value ? null : reader["address"].ToString(),
                Email = reader["email"].ToString(),
                Phone = reader["phonenumber"].ToString(),
                Country = reader["location"] == DBNull.Value ? null : reader["location"].ToString(),
                Birthday = Convert.ToDateTime(reader["dateOfBirth"]),
                Gender = reader["gender"].ToString(),
                Login = reader["login"].ToString(),
                RoleId = (int)reader["roleId"],
            };
        }
    }
}
