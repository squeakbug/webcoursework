using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DataAccessInterface;
using Presenter;

namespace AuthService
{
    public class AuthService : IAuthService
    {
        private IRepositoryFactory _repositoryFactory;

        public AuthService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException("repository factory");
        }

        public async Task<UserInfo> GetUserById(int userId)
        {
            UserInfo user;
            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                user = await repo.GetUserById(userId);
            }
            return user;
        }
        public async Task UpdateUserPassword(int userId, string newPassword)
        {
            if (newPassword == null)
                throw new ClientErrorException("new password must not be null");
            UserInfo user;
            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                user = await repo.GetUserById(userId);
            }
            if (user == null)
                throw new NotFoundException($"no user with id = {userId}");
            if (user.Loggined == false)
                throw new ClientErrorException($"client with id = {userId} not loggined");
            user.Password = newPassword;

            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                await repo.Update(user);
            }
        }
        public async Task UpdateUserName(int userId, string newName)
        {
            if (newName == null)
                throw new ClientErrorException("new name must not be null");
            UserInfo user;
            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                user = await repo.GetUserById(userId);
            }

            if (user == null)
                throw new NotFoundException($"no user with id = {userId}");
            if (user.Loggined == false)
                throw new ClientErrorException($"client with id = {userId} not loggined");
            user.Name = newName;

            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                await repo.Update(user);
            }
        }
        public async Task<int> LoginUser(string login, string password)
        {
            UserInfo user;
            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                user = await repo.GetUserByLogin(login);
            }

            if (user == null)
                throw new NotFoundException($"no user with login = {login}");
            if (user.Password != password)
                throw new ClientErrorException($"password is not correct");
            if (user.Loggined == true)
                throw new ClientErrorException($"user with id = {user.Id} already loggined");
            user.Loggined = true;

            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                await repo.Update(user);
            }
            return user.Id;
        }
        public async Task LogoutUser(int userId)
        {
            UserInfo user;
            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                user = await repo.GetUserById(userId);
            }

            if (user == null)
                throw new ApplicationException($"no user with id = {userId}");
            if (user.Loggined == false)
                throw new ApplicationException($"user with id = {userId} not yet loggined");
            user.Loggined = false;

            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                await repo.Update(user);
            }
        }
        public async Task<int> RegistrateUser(string login, string password, string repPassword)
        {
            if (password != repPassword)
                throw new ApplicationException("password != repPassword");

            UserInfo user;
            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                user = await repo.GetUserByLogin(login);
            }

            if (user != null)
                throw new ApplicationException("user already exists");
            var newUser = new UserInfo
            {
                Loggined = false,
                Login = login,
                Name = "default",
                Password = password,
            };

            int id;
            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                id = await repo.Create(newUser);
            }
            return id;
        }

        public async Task<IEnumerable<UserInfo>> GetUsers()
        {
            IEnumerable<UserInfo> users;
            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                users = await repo.GetUserInfos();
            }
            return users;
        }

        public async Task<bool> IsUserLoggined(int userId)
        {
            UserInfo user;
            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                user = await repo.GetUserById(userId);
            }

            if (user == null)
                throw new NotFoundException($"no user with id = {userId}");

            return user.Loggined;
        }
    }
}
