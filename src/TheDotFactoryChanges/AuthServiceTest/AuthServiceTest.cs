using Moq;

using DataAccessInterface;
using AuthService;
using NUnit.Allure.Core;
using Allure.Commons;
using NUnit.Allure.Attributes;
using NUnit.Framework;

namespace AuthServiceTest
{
    [AllureNUnit]
    public class AuthServiceTest
    {
        [Test]
        public void RegistrateBadRepeatPassword()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            var sut = new AuthService.AuthService(repoMock.Object);

            TestDelegate action = () => sut.RegistrateUser("user", "pass", "bad_pass");

            Assert.Throws<ApplicationException>(action);
        }

        [Test]
        public void RegistrateGoodRepeatPassword()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            var sut = new AuthService.AuthService(repoMock.Object);

            sut.RegistrateUser("user", "pass", "pass");

            userRepoMock.Verify(repo =>
                repo.Create(It.Is<UserInfo>(info =>
                    info.Login == "user" && info.Name == "default")), Times.Once);
        }

        [Test]
        public void LoginRegisteredUser()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];
            userRepoMock.Setup(repo => repo.GetUserByLogin(user.Name))
                        .Returns(user);
            userRepoMock.Setup(repo => repo.Update(It.IsAny<UserInfo>()));
            var sut = new AuthService.AuthService(repoMock.Object);

            sut.LoginUser(user.Name, "pass");

            userRepoMock.Verify(repo =>
                repo.Update(It.Is<UserInfo>(info =>
                    info.Loggined == true)));
        }

        [Test]
        public void LoginUnRegisteredUser()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];
            userRepoMock.Setup(repo => repo.GetUserByLogin(user.Name))
                        .Returns(user);
            userRepoMock.Setup(repo => repo.Update(It.IsAny<UserInfo>()));
            var sut = new AuthService.AuthService(repoMock.Object);

            TestDelegate del = () => sut.LoginUser("user", "pass");

            Assert.Throws<NotFoundException>(del);
        }

        [Test]
        public void LogoutLogginedUser()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];
            user.Loggined = true;
            userRepoMock.Setup(repo => repo.GetUserById(user.Id))
                        .Returns(user);
            userRepoMock.Setup(repo => repo.Update(It.IsAny<UserInfo>()));
            var sut = new AuthService.AuthService(repoMock.Object);

            sut.LogoutUser(user.Id);

            userRepoMock.Verify(repo =>
                repo.Update(It.Is<UserInfo>(info =>
                    info.Loggined == false)));
        }

        [Test]
        public void LogoutUnlogginedUser()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];
            userRepoMock.Setup(repo => repo.GetUserById(user.Id))
                        .Returns(user);
            userRepoMock.Setup(repo => repo.Update(It.IsAny<UserInfo>()));
            var sut = new AuthService.AuthService(repoMock.Object);

            TestDelegate del = () => sut.LogoutUser(user.Id);

            Assert.Throws<ApplicationException>(del);
        }

        [Test]
        public void LogoutUnregisteredUser()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];
            user.Loggined = true;
            userRepoMock.Setup(repo => repo.GetUserByLogin(user.Name))
                        .Returns((UserInfo)null);
            userRepoMock.Setup(repo => repo.Update(It.IsAny<UserInfo>()));
            var sut = new AuthService.AuthService(repoMock.Object);

            TestDelegate del = () => sut.LogoutUser(user.Id);

            Assert.Throws<ApplicationException>(del);
        }

        [Test]
        public void GetUserByIdDefaultTest()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];
            userRepoMock.Setup(repo => repo.GetUserById(It.IsAny<int>()))
                        .Returns(user);
            var sut = new AuthService.AuthService(repoMock.Object);

            var retUser = sut.GetUserById(user.Id);

            userRepoMock.Verify(repo =>
                repo.GetUserById(It.Is<int>(id =>
                    id == user.Id)));
            Assert.AreEqual(retUser.Id, user.Id);
        }

        [Test]
        public void UpdateUnlogginedUserPasswordTest()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];
            userRepoMock.Setup(repo => repo.GetUserById(user.Id))
                        .Returns(user);
            userRepoMock.Setup(repo => repo.Update(It.IsAny<UserInfo>()));
            var sut = new AuthService.AuthService(repoMock.Object);

            TestDelegate del = () => sut.UpdateUserPassword(user.Id, "newPass");

            Assert.Throws<ClientErrorException>(del);
        }

        [Test]
        public void UpdateLogginedUserPasswordTest()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];
            user.Loggined = true;
            userRepoMock.Setup(repo => repo.GetUserById(user.Id))
                        .Returns(user);
            userRepoMock.Setup(repo => repo.Update(It.IsAny<UserInfo>()));
            var sut = new AuthService.AuthService(repoMock.Object);

            sut.UpdateUserPassword(user.Id, "newPass");

            userRepoMock.Verify(repo =>
                repo.Update(It.Is<UserInfo>(info =>
                    info.Password == "newPass")));
        }

        [Test]
        public void UpdateUnregisteredUserPasswordTest()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];
            userRepoMock.Setup(repo => repo.GetUserById(user.Id))
                        .Returns((UserInfo)null);
            userRepoMock.Setup(repo => repo.Update(It.IsAny<UserInfo>()));
            var sut = new AuthService.AuthService(repoMock.Object);

            TestDelegate del = () => sut.UpdateUserPassword(user.Id, "newPass");

            Assert.Throws<NotFoundException>(del);
        }

        [Test]
        public void UpdateToNullUserPasswordTest()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];
            user.Loggined = true;
            userRepoMock.Setup(repo => repo.GetUserById(user.Id))
                        .Returns(user);
            userRepoMock.Setup(repo => repo.Update(It.IsAny<UserInfo>()));
            var sut = new AuthService.AuthService(repoMock.Object);

            TestDelegate del = () => sut.UpdateUserPassword(user.Id, null);

            Assert.Throws<ClientErrorException>(del);
        }

        [Test]
        public void UpdateUserNameTest()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];
            user.Loggined = true;
            userRepoMock.Setup(repo => repo.GetUserById(user.Id))
                        .Returns(user);
            user.Name = "test";
            userRepoMock.Setup(repo => repo.Update(user));
            var sut = new AuthService.AuthService(repoMock.Object);

            sut.UpdateUserName(user.Id, "test");

            userRepoMock.Verify(r => r.Update(user));
        }

        [Test]
        public void IsUserLogginedUnregisteredUser()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            var user = GetUnlogginedUserInfoSample()[0];
            userRepoMock.Setup(repo => repo.GetUserById(user.Id))
                        .Returns((UserInfo) null);
            var sut = new AuthService.AuthService(repoMock.Object);

            TestDelegate del = () => sut.IsUserLoggined(user.Id);

            Assert.Throws<NotFoundException>(del);
        }

        [Test]
        public void GetUsersDefaultTest()
        {
            var userRepoMock = new Mock<IUserRepository>();
            var repoMock = new Mock<IRepositoryFactory>();
            repoMock.Setup(factory => factory.CreateUserRepository())
                    .Returns(userRepoMock.Object);
            IEnumerable<UserInfo> users = GetUnlogginedUserInfoSample();
            userRepoMock.Setup(repo => repo.GetUserInfos())
                        .Returns(users);
            var sut = new AuthService.AuthService(repoMock.Object);

            var result = sut.GetUsers();

            userRepoMock.Verify(repo => repo.GetUserInfos());
        }

        private List<UserInfo> GetUnlogginedUserInfoSample()
        {
            return new List<UserInfo>()
            {
                new UserInfo
                {
                    Id = 1,
                    Loggined = false,
                    Login = "user_001",
                    Name = "user_001",
                    Password = "pass",
                },
                new UserInfo
                {
                    Id = 2,
                    Loggined = false,
                    Login = "user_002",
                    Name = "user_002",
                    Password = "pass",
                },
                new UserInfo
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