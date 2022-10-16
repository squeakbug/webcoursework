using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using DataAccessInterface;

namespace DataAccessSQLServer
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public string ConnectionString
        {
            get
            {
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = DataSource,
                    InitialCatalog = InitialCatalog,
                    UserID = User,
                    Password = Password,
                };
                return builder.ToString();
            }
        }

        public RepositoryFactory(string dataSource, string initialCatalog, string user, string passwd)
        {
            DataSource = dataSource;
            InitialCatalog = initialCatalog;
            User = user;
            Password = passwd;
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
        public IConfigRepository CreateConfigRepository()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), ConnectionString)
                .Options;
            var globalCtx = new Context(options);
            globalCtx.Database.EnsureCreated();
            return new ConfigRepository(globalCtx);
        }

        public IConvertionRepository CreateConvertionRepository()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), ConnectionString)
                .Options;
            var globalCtx = new Context(options);
            globalCtx.Database.EnsureCreated();
            return new ConvertionRepository(globalCtx);
        }

        public IFontRepository CreateFontRepository()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), ConnectionString)
                .Options;
            var globalCtx = new Context(options);
            globalCtx.Database.EnsureCreated();
            return new FontRepository(globalCtx);
        }
    }
}
