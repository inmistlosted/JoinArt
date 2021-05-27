using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using diplom.api.DataAccessLayer;
using diplom.api.Models;
using diplom.api.Models.ResponseModels;
using diplom.api.Providers;
using diplom.api.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace diplom.api.Controllers
{
    [Route("users")]
    public class UsersController : BaseController
    {
        private readonly IUserProvider _userProvider;

        public UsersController(IDataAccessAdapter dataAccessAdapter, IMemoryCache cache, IOptions<AppSettings> settings, IUserProvider userProvider)
            : base(dataAccessAdapter, cache, settings)
        {
            this._userProvider = userProvider ?? throw new ArgumentNullException(nameof(userProvider));
        }

        [HttpPost, Route("register")]
        public async Task<IActionResult> RegisterUser(RegisterModel model)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            RegisterResponseModel response = await _userProvider.AddUser(model);

            return Json(response);
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> LoginUser(LoginModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            LoginResponseModel response = await _userProvider.LoginUser(model);

            return Json(response);
        }

        [HttpPost, Route("follow/{followerId}/{followingId}")]
        public async Task<IActionResult> FollowUser(int followerId, int followingId)
        {
            if (followerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(followerId));
            }

            if (followingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(followingId));
            }

            await _userProvider.FollowUser(followerId, followingId);

            return Ok();
        }

        [HttpPost, Route("unfollow/{followerId}/{followingId}")]
        public async Task<IActionResult> UnfollowUser(int followerId, int followingId)
        {
            if (followerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(followerId));
            }

            if (followingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(followingId));
            }

            await _userProvider.UnfollowUser(followerId, followingId);

            return Ok();
        }

        [HttpGet, Route("get-user/{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            User user = await _userProvider.GetUser(userId);

            return Json(user);
        }

        [HttpGet, Route("get-user-info/{userId}/{currentUserId}")]
        public async Task<IActionResult> GetUserInfo(int userId, int currentUserId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            if (currentUserId < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(currentUserId));
            }

            User user = await _userProvider.GetUser(userId);
            bool isFollowing = false;

            if(currentUserId > 0)
            {
                isFollowing = await _userProvider.IsFollowing(currentUserId, userId);
            }

            UserInfoModel response = new UserInfoModel
            {
                Login = user.Login,
                IsFollowing = isFollowing,
            };

            return Json(response);
        }

        [HttpPost, Route("update")]
        public async Task<IActionResult> UpdateUser(RegisterModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            RegisterResponseModel response = await _userProvider.UpdateUser(model);

            return Json(response);
        }
    }
}
