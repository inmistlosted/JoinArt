using diplom.api.Models;
using diplom.api.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.Providers
{
    public interface IUserProvider
    {
        Task<RegisterResponseModel> AddUser(RegisterModel registerModel);
        Task<RegisterResponseModel> UpdateUser(RegisterModel registerModel);
        Task<LoginResponseModel> LoginUser(LoginModel loginModel);
        Task<User> GetPaintingOwner(int paintingId, bool withCache = false);
        Task<bool> IsFollowing(int userId1, int userId2);
        Task FollowUser(int userId1, int userId2);
        Task UnfollowUser(int userId1, int userId2);
        Task<User> GetUser(int userId, bool withCache = false);
    }
}
