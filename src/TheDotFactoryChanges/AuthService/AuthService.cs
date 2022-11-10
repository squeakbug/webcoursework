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
            var repo = _repositoryFactory.CreateUserRepository();
            return await repo.GetUserById(userId);
        }
        public async Task UpdateUserPassword(int userId, string newPassword)
        {
            if (newPassword == null)
                throw new ClientErrorException("new password must not be null");
            var repo = _repositoryFactory.CreateUserRepository();
            var user = await repo.GetUserById(userId);
            if (user == null)
                throw new NotFoundException($"no user with id = {userId}");
            if (user.Loggined == false)
                throw new ClientErrorException($"client with id = {userId} not loggined");
            user.Password = newPassword;

            repo = _repositoryFactory.CreateUserRepository();
            await repo.Update(user);
        }
        public async Task UpdateUserName(int userId, string newName)
        {
            if (newName == null)
                throw new ClientErrorException("new name must not be null");
            var repo = _repositoryFactory.CreateUserRepository();
            var user = await repo.GetUserById(userId);
            if (user == null)
                throw new NotFoundException($"no user with id = {userId}");
            if (user.Loggined == false)
                throw new ClientErrorException($"client with id = {userId} not loggined");
            user.Name = newName;

            repo = _repositoryFactory.CreateUserRepository();
            await repo.Update(user);
        }
        public async Task<int> LoginUser(string login, string password)
        {
            var repo = _repositoryFactory.CreateUserRepository();
            var user = await repo.GetUserByLogin(login);
            if (user == null)
                throw new NotFoundException($"no user with login = {login}");
            if (user.Password != password)
                throw new ClientErrorException($"password is not correct");
            if (user.Loggined == true)
                throw new ClientErrorException($"user with id = {user.Id} already loggined");
            user.Loggined = true;

            repo = _repositoryFactory.CreateUserRepository();
            await repo.Update(user);
            return user.Id;
        }
        public async Task LogoutUser(int userId)
        {
            var repo = _repositoryFactory.CreateUserRepository();
            var user = await repo.GetUserById(userId);
            if (user == null)
                throw new ApplicationException($"no user with id = {userId}");
            if (user.Loggined == false)
                throw new ApplicationException($"user with id = {userId} not yet loggined");
            user.Loggined = false;

            repo = _repositoryFactory.CreateUserRepository();
            await repo.Update(user);
        }
        public async Task<int> RegistrateUser(string login, string password, string repPassword)
        {
            if (password != repPassword)
                throw new ApplicationException("password != repPassword");
            var repo = _repositoryFactory.CreateUserRepository();

            var user = await repo.GetUserByLogin(login);
            if (user != null)
                throw new ApplicationException("user already exists");

            var newUser = new UserInfo
            {
                Loggined = false,
                Login = login,
                Name = "default",
                Password = password,
            };
            repo = _repositoryFactory.CreateUserRepository();
            return await repo.Create(newUser);
        }

        public async Task<IEnumerable<UserInfo>> GetUsers()
        {
            var repo = _repositoryFactory.CreateUserRepository();
            return await repo.GetUserInfos();
        }

        public async Task<bool> IsUserLoggined(int userId)
        {
            var repo = _repositoryFactory.CreateUserRepository();
            var user = await repo.GetUserById(userId);
            if (user == null)
                throw new NotFoundException($"no user with id = {userId}");
            return user.Loggined;
        }
    }
}
