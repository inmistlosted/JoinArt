using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer.Implementation
{
    public class UserAdapter : IUserAdapter
    {
        private ICommandAdapter _commandAdapter;

        public UserAdapter(ICommandAdapter commandAdapter)
        {
            this._commandAdapter = commandAdapter ?? throw new ArgumentNullException(nameof(commandAdapter));
        }

        public async Task<IDataReader> AddUser(string firstName, string secondName, string address, string email, string phone, string location, DateTime dateOfBirth, string gender, string login, string passwordHash)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (string.IsNullOrEmpty(secondName))
            {
                throw new ArgumentNullException(nameof(secondName));
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (string.IsNullOrEmpty(phone))
            {
                throw new ArgumentNullException(nameof(phone));
            }

            if (string.IsNullOrEmpty(gender))
            {
                throw new ArgumentNullException(nameof(gender));
            }

            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (string.IsNullOrEmpty(passwordHash))
            {
                throw new ArgumentNullException(nameof(passwordHash));
            }

            using (NpgsqlCommand sqlCommand = CreateAddUserCommand(firstName, secondName, address, email, phone, location, dateOfBirth, gender, login, passwordHash))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task UpdateUser(int userId, string firstName, string secondName, string address, string email, string phone, string location, DateTime dateOfBirth, string gender, string login)
        {
            if(userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (string.IsNullOrEmpty(secondName))
            {
                throw new ArgumentNullException(nameof(secondName));
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (string.IsNullOrEmpty(phone))
            {
                throw new ArgumentNullException(nameof(phone));
            }

            if (string.IsNullOrEmpty(gender))
            {
                throw new ArgumentNullException(nameof(gender));
            }

            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentNullException(nameof(login));
            }

            using (NpgsqlCommand sqlCommand = CreateUpdateUserCommand(userId, firstName, secondName, address, email, phone, location, dateOfBirth, gender, login))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetUserRole(int userId)
        {
            if(userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetUserRoleCommand(userId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetUserRole(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentNullException(nameof(login));
            }

            using (NpgsqlCommand sqlCommand = CreateGetUserRoleCommand(login))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetUserId(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentNullException(nameof(login));
            }

            using (NpgsqlCommand sqlCommand = CreateGetUserIdCommand(login))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> UserExists(string login, string email)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentNullException(nameof(login));
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            using (NpgsqlCommand sqlCommand = CreateUserExistsCommand(login, email))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetUserPasswordByLogin(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentNullException(nameof(login));
            }

            using (NpgsqlCommand sqlCommand = CreateGetUserPasswordByLoginCommand(login))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetPaintingOwner(int paintingId)
        {
            if (paintingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paintingId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetPaintingOwnerCommand(paintingId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> IsFollowingPaintingOwner(int userId, int ownerId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            if (ownerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ownerId));
            }

            using (NpgsqlCommand sqlCommand = CreateIsFollowingPaintingOwnerCommand(userId, ownerId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task FollowUser(int followerId, int followingId)
        {
            if (followerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(followerId));
            }

            if (followingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(followingId));
            }

            using (NpgsqlCommand sqlCommand = CreateFollowUserCommand(followerId, followingId))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task UnfollowUser(int followerId, int followingId)
        {
            if (followerId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(followerId));
            }

            if (followingId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(followingId));
            }

            using (NpgsqlCommand sqlCommand = CreateUnfollowUserCommand(followerId, followingId))
            {
                await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetUser(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetUserCommand(userId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetUserFollowers(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetUserFollowersCommand(userId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        public async Task<IDataReader> GetUserFollowing(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            using (NpgsqlCommand sqlCommand = CreateGetUserFollowingCommand(userId))
            {
                return await this._commandAdapter.ExecuteReaderAsync(sqlCommand);
            }
        }

        private NpgsqlCommand CreateAddUserCommand(string firstName, string secondName, string address, string email, string phone, string location, DateTime dateOfBirth, string gender, string login, string passwordHash)
        {
            string query = @"insert into users (firstname, lastname, address, email, phonenumber, location, dateofbirth, gender, login, password, roleId)
                             values (@firstname, @lastname, @address, @email, @phonenumber, @location, @dateofbirth, @gender, @login, @password, 1)
                             returning userId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("firstname", firstName);
            sqlCommand.Parameters.AddWithValue("lastname", secondName);
            sqlCommand.Parameters.AddWithValue("address", address ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("email", email);
            sqlCommand.Parameters.AddWithValue("phonenumber", phone);
            sqlCommand.Parameters.AddWithValue("location", location ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("dateofbirth", dateOfBirth > DateTime.MinValue ? (object)dateOfBirth : DBNull.Value);
            sqlCommand.Parameters.AddWithValue("gender", gender);
            sqlCommand.Parameters.AddWithValue("login", login);
            sqlCommand.Parameters.AddWithValue("password", passwordHash);

            return sqlCommand;
        }

        private NpgsqlCommand CreateUpdateUserCommand(int userId, string firstName, string secondName, string address, string email, string phone, string location, DateTime dateOfBirth, string gender, string login)
        {
            string query = @"update users set firstname = @firstname, lastname = @lastname, address = @address, email = @email, phonenumber = @phonenumber, location = @location, dateofbirth = @dateofbirth, gender = @gender, login = @login
                             where userId = @userId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("firstname", firstName);
            sqlCommand.Parameters.AddWithValue("lastname", secondName);
            sqlCommand.Parameters.AddWithValue("address", address ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("email", email);
            sqlCommand.Parameters.AddWithValue("phonenumber", phone);
            sqlCommand.Parameters.AddWithValue("location", location ?? (object)DBNull.Value);
            sqlCommand.Parameters.AddWithValue("dateofbirth", dateOfBirth > DateTime.MinValue ? (object)dateOfBirth : DBNull.Value);
            sqlCommand.Parameters.AddWithValue("gender", gender);
            sqlCommand.Parameters.AddWithValue("login", login);
            sqlCommand.Parameters.AddWithValue("userId", userId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetUserRoleCommand(int userId)
        {
            string query = @"select roleId from users where userId = @userId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("userId", userId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetUserRoleCommand(string login)
        {
            string query = @"select roleId from users where login = @login";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("login", login);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetUserIdCommand(string login)
        {
            string query = @"select userId from users where login = @login";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("login", login);

            return sqlCommand;
        }

        private NpgsqlCommand CreateUserExistsCommand(string login, string email)
        {
            string query = @"select userId from users where login = @login or email = @email";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("login", login);
            sqlCommand.Parameters.AddWithValue("email", email);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetUserPasswordByLoginCommand(string login)
        {
            string query = @"select password from users where login = @login";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("login", login);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetPaintingOwnerCommand(int paintingId)
        {
            string query = @"select * 
                             from paintingsusers inner join users on paintingsusers.userId = users.userId 
                             where paintingId = @paintingId;";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("paintingId",paintingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateIsFollowingPaintingOwnerCommand(int userId, int ownerId)
        {
            string query = @"select followerid from follows where followerid = @userId and followingid = @ownerId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("userId", userId);
            sqlCommand.Parameters.AddWithValue("ownerId", ownerId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateFollowUserCommand(int followerId, int followingId)
        {
            string query = @"insert into follows (followerid, followingid) 
                             select @followerid, @followingid 
                             where not exists (
                                    select * 
                                    from follows 
                                    where followerid = @followerid and followingid = @followingid)";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("followerid", followerId);
            sqlCommand.Parameters.AddWithValue("followingid", followingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateUnfollowUserCommand(int followerId, int followingId)
        {
            string query = @"delete from follows where followerid = @followerid and followingid = @followingid";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("followerid", followerId);
            sqlCommand.Parameters.AddWithValue("followingid", followingId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetUserCommand(int userId)
        {
            string query = @"select * from users where userId = @userId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("userId", userId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetUserFollowersCommand(int userId)
        {
            string query = @"select * 
                             from follows inner join users on follows.followerid = users.userid 
                             where followingid = @userId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("userId", userId);

            return sqlCommand;
        }

        private NpgsqlCommand CreateGetUserFollowingCommand(int userId)
        {
            string query = @"select * 
                             from follows inner join users on follows.followingid = users.userid 
                             where followerid = @userId";

            var sqlCommand = new NpgsqlCommand(query);

            sqlCommand.Parameters.AddWithValue("userId", userId);

            return sqlCommand;
        }
    }
}
