using System.Threading.Tasks;

using NUnit.Allure.Core;
using NUnit.Framework;

using AuthService;
using DataAccessSQLServer;

namespace CommonITCase
{
    [AllureNUnit]
    public class Tests
    {

        [Test]
        public async Task LoginIntegrationTest()
        {
            Common.CreateTestDatabase("the_dotfactory_test", "127.0.0.1");
            var snapshotName = "testdb";
            var snapshotPath = $@"/var/opt/mssql/data/{snapshotName}.ss";
            Common.CreateDatabaseSnapshot("the_dotfactory_test", "127.0.0.1", snapshotName, snapshotPath);
            var contextFactory = new DbContextFactory();
            var repoFactory = new RepositoryFactory(contextFactory, "127.0.0.1", "the_dotfactory_test", "SA", "P@ssword", false);
            var textRenderer = new WinFormsTextRendererAdapter();
            var authService = new AuthService.AuthService(repoFactory);
            await authService.RegistrateUser("user_001", "password", "password");

            var userId = await authService.LoginUser("user_001", "password");

            var userRepo = repoFactory.CreateUserRepository();
            var user = await userRepo.GetUserById(userId);
            userRepo.Dispose();
            Assert.NotNull(user);
            Assert.AreEqual(user.Loggined, true);
            Assert.AreEqual(user.Login, "user_001");
            Assert.AreEqual(user.Name, "default");
            Assert.AreEqual(user.Password, "password");
            Common.RestoreDatabaseBySnapshot("the_dotfactory_test", "127.0.0.1", snapshotName);
        }
    }
}