using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer
{
    public interface IUserAdapter
    {
        Task<IDataReader> AddUser(string firstName, string secondName, string address, string email, string phone, string location, DateTime dateOfBirth, string gender, string login, string passwordHash);
        Task UpdateUser(int userId, string firstName, string secondName, string address, string email, string phone, string location, DateTime dateOfBirth, string gender, string login);
        Task<IDataReader> GetUserRole(int userId);
        Task<IDataReader> GetUserRole(string login);
        Task<IDataReader> GetUserId(string login);
        Task<IDataReader> GetUserPasswordByLogin(string login);
        Task<IDataReader> UserExists(string login, string email);
        Task<IDataReader> GetPaintingOwner(int paintingId);
        Task<IDataReader> IsFollowingPaintingOwner(int userId, int ownerId);
        Task FollowUser(int followerId, int followingId);
        Task UnfollowUser(int followerId, int followingId);
        Task<IDataReader> GetUser(int userId);
        Task<IDataReader> GetUserFollowers(int userId);
        Task<IDataReader> GetUserFollowing(int userId);
    }
}
