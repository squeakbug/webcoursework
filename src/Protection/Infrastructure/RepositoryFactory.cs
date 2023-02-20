using Microsoft.EntityFrameworkCore;

using Domain;

namespace Infrastructure
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public string ConnectionString { get; private set; }

        public RepositoryFactory(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public IUserRepository CreateUserRepository()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), ConnectionString)
                .Options;
            var globalCtx = new Context(options);
            globalCtx.Database.EnsureCreated();
            return new UserRepository(globalCtx);
        }
    }
}
