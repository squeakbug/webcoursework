using System.Linq;

using Moq;
using Microsoft.EntityFrameworkCore;

using DataAccessSQLServer;
using DataAccessInterface;

// https://learn.microsoft.com/ru-ru/ef/ef6/fundamentals/testing/mocking

namespace SqlServerDALTest
{
    public class UserRepositoryTest
    {
        [Test]
        public void CreateUserDefaultTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var userDbSetMock = new Mock<DbSet<DataAccessSQLServer.UserInfo>>();
            var userSetMock = new Mock<Context>(options);
            userSetMock.Setup(u => u.UserInfo)
                       .Returns(userDbSetMock.Object);
            userSetMock.Setup(set =>
                set.UserInfo.Add(It.IsAny<DataAccessSQLServer.UserInfo>()));
            userSetMock.Setup(set =>
                set.SaveChanges());
            var sut = new UserRepository(userSetMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];

            sut.Create(user);

            userSetMock.Verify(set =>
                set.UserInfo.Add(It.Is<DataAccessSQLServer.UserInfo>(info =>
                    info.Usr_login == user.Login)));
            userSetMock.Verify(set =>
                set.SaveChanges());
        }

        [Test]
        public void GetUserByIdTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var userDbSetMock = new Mock<DbSet<DataAccessSQLServer.UserInfo>>();
            var userSetMock = new Mock<Context>(options);
            var user = GetUnlogginedUserInfoSample()[0];
            userSetMock.Setup(u => u.UserInfo)
                       .Returns(userDbSetMock.Object);
            userSetMock.Setup(set =>
                set.UserInfo.Find(It.IsAny<int>()))
                       .Returns(new DataAccessSQLServer.UserInfo
                            { Id = user.Id, Loggined = user.Loggined,
                              Usr_login = user.Login, Usr_name = user.Name,
                              Usr_passwd = user.Password });
            var sut = new UserRepository(userSetMock.Object);

            var retUser = sut.GetUserById(user.Id);

            userSetMock.Verify(set =>
                set.UserInfo.Find(It.Is<int>(id =>
                    id == user.Id)));
        }

        [Test]
        public void GetUserByLoginTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var userDbSetMock = new Mock<DbSet<DataAccessSQLServer.UserInfo>>();
            var userSetMock = new Mock<Context>(options);
            var users = GetDALUnlogginedUserInfoSample();
            userSetMock.Setup(u => u.GetUserInfoSet())
                       .Returns(users.AsQueryable());
            var sut = new UserRepository(userSetMock.Object);

            var retUser = sut.GetUserByLogin("user_001");

            userSetMock.Verify(set => set.GetUserInfoSet());
            Assert.NotNull(retUser);
        }

        [Test]
        public void GetUsersTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var userDbSetMock = new Mock<DbSet<DataAccessSQLServer.UserInfo>>();
            var userSetMock = new Mock<Context>(options);
            userSetMock.Setup(u => u.UserInfo)
                       .Returns(userDbSetMock.Object);
            var users = GetDALUnlogginedUserInfoSample();
            userSetMock.Setup(u => u.GetUserInfoSet())
                       .Returns(users.AsQueryable());
            var sut = new UserRepository(userSetMock.Object);

            var retUser = sut.GetUserInfos();

            userSetMock.Verify(set => set.GetUserInfoSet());
        }

        [Test]
        public void UpdateUserTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var userDbSetMock = new Mock<DbSet<DataAccessSQLServer.UserInfo>>();
            var userSetMock = new Mock<Context>(options);
            userSetMock.Setup(u => u.UserInfo)
                       .Returns(userDbSetMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];
            var sut = new UserRepository(userSetMock.Object);

            sut.Update(user);

            userSetMock.Verify(set => set.UserInfo);
        }

        [Test]
        public void DeleteUserTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var userDbSetMock = new Mock<DbSet<DataAccessSQLServer.UserInfo>>();
            var userSetMock = new Mock<Context>(options);
            var user = GetUnlogginedUserInfoSample()[0];
            userSetMock.Setup(u => u.UserInfo)
                       .Returns(userDbSetMock.Object);
            userSetMock.Setup(set =>
                set.UserInfo.Find(It.IsAny<int>()))
                       .Returns(new DataAccessSQLServer.UserInfo
                       {
                           Id = user.Id,
                           Loggined = user.Loggined,
                           Usr_login = user.Login,
                           Usr_name = user.Name,
                           Usr_passwd = user.Password
                       });
            userSetMock.Setup(set =>
                set.UserInfo.Remove(It.IsAny<DataAccessSQLServer.UserInfo>()));
            var sut = new UserRepository(userSetMock.Object);

            sut.Delete(user.Id);

            userSetMock.Verify(set => 
                set.UserInfo.Remove(It.IsAny<DataAccessSQLServer.UserInfo>()));
        }

        private List<DataAccessSQLServer.UserInfo> GetDALUnlogginedUserInfoSample()
        {
            return new List<DataAccessSQLServer.UserInfo>()
            {
                new DataAccessSQLServer.UserInfo
                {
                    Id = 1,
                    Loggined = false,
                    Usr_login = "user_001",
                    Usr_name = "user_001",
                    Usr_passwd = "pass",
                },
                new DataAccessSQLServer.UserInfo
                {
                    Id = 2,
                    Loggined = false,
                    Usr_login = "user_002",
                    Usr_name = "user_002",
                    Usr_passwd = "pass",
                },
                new DataAccessSQLServer.UserInfo
                {
                    Id = 3,
                    Loggined = false,
                    Usr_login = "user_003",
                    Usr_name = "user_003",
                    Usr_passwd = "pass",
                }
            };
        }
        private List<DataAccessInterface.UserInfo> GetUnlogginedUserInfoSample()
        {
            return new List<DataAccessInterface.UserInfo>()
            {
                new DataAccessInterface.UserInfo
                {
                    Id = 1,
                    Loggined = false,
                    Login = "user_001",
                    Name = "user_001",
                    Password = "pass",
                },
                new DataAccessInterface.UserInfo
                {
                    Id = 2,
                    Loggined = false,
                    Login = "user_002",
                    Name = "user_002",
                    Password = "pass",
                },
                new DataAccessInterface.UserInfo
                {
                    Id = 3,
                    Loggined = false,
                    Login = "user_003",
                    Name = "user_003",
                    Password = "pass",
                }
            };
        }
    }
}
