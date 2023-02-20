using Domain;

namespace Application
{
    public class UserService : IUserService
    {
        private IRepositoryFactory _repositoryFactory;

        public UserService(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory ?? throw new ArgumentNullException(nameof(repositoryFactory));
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            IEnumerable<User> users;
            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                users = await repo.GetUsers();
            }
            return users;
        }
        public async Task<User?> GetUserById(Guid id)
        {
            User? user;
            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                user = await repo.GetUserById(id);
            }
            return user;
        }
        public async Task<User?> Create(NewUser newUser)
        {
            User? user;
            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                user = await repo.Create(newUser);
            }
            return user;
        }
        public async Task<User?> Update(User user)
        {
            User? newUser;
            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                newUser = await repo.Update(user);
            }
            return newUser;
        }
        public async Task<bool> Delete(Guid id)
        {
            bool deleted;
            using (var repo = _repositoryFactory.CreateUserRepository())
            {
                deleted = await repo.Delete(id);
            }
            return deleted;
        }
    }
}