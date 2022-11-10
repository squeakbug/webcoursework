using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Moq;
using Microsoft.EntityFrameworkCore;
using NUnit.Allure.Core;
using NUnit.Framework;

using DataAccessSQLServer;
using DataAccessInterface;


// https://learn.microsoft.com/ru-ru/ef/ef6/fundamentals/testing/mocking

namespace SQLServerDALTest
{
    [AllureNUnit]
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
                set.SaveChangesAsync());
            var sut = new UserRepository(userSetMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];

            sut.Create(user);

            userSetMock.Verify(set =>
                set.UserInfo.Add(It.Is<DataAccessSQLServer.UserInfo>(info =>
                    info.Usr_login == user.Login)));
            userSetMock.Verify(set =>
                set.SaveChangesAsync());
        }

        [Test]
        public async Task GetUserByIdTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var userDbSetMock = new Mock<DbSet<DataAccessSQLServer.UserInfo>>();
            var userSetMock = new Mock<Context>(options);
            var user = GetUnlogginedUserInfoSample()[0];
            userSetMock.Setup(u => u.UserInfo)
                       .Returns(userDbSetMock.Object);
            var userInfoSample = new DataAccessSQLServer.UserInfo
            {
                Id = user.Id,
                Usr_Loggined = user.Loggined,
                Usr_login = user.Login,
                Usr_name = user.Name,
                Usr_passwd = user.Password
            };
            userSetMock.Setup(set =>
                set.UserInfo.FindAsync(It.IsAny<int>()))
                       .ReturnsAsync(userInfoSample);
            var sut = new UserRepository(userSetMock.Object);

            var retUser = await sut.GetUserById(user.Id);

            userSetMock.Verify(set =>
                set.UserInfo.FindAsync(It.Is<int>(id =>
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
            var userInfoSample = new DataAccessSQLServer.UserInfo
            {
                Id = user.Id,
                Usr_Loggined = user.Loggined,
                Usr_login = user.Login,
                Usr_name = user.Name,
                Usr_passwd = user.Password
            };
            userSetMock.Setup(set =>
                set.UserInfo.FindAsync(It.IsAny<int>()))
                       .ReturnsAsync(userInfoSample);
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
                    Usr_Loggined = false,
                    Usr_login = "user_001",
                    Usr_name = "user_001",
                    Usr_passwd = "pass",
                },
                new DataAccessSQLServer.UserInfo
                {
                    Id = 2,
                    Usr_Loggined = false,
                    Usr_login = "user_002",
                    Usr_name = "user_002",
                    Usr_passwd = "pass",
                },
                new DataAccessSQLServer.UserInfo
                {
                    Id = 3,
                    Usr_Loggined = false,
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
