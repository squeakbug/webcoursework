using System.Threading.Tasks;

using NUnit.Allure.Core;
using NUnit.Framework;

using AuthService;
using DataAccessSQLServer;
using System;

namespace CommonITCase
{
    [AllureNUnit]
    public class Tests
    {

        [Test]
        public async Task LoginIntegrationTest()
        {
            var serverAddr = "webapp_sqlserver";
            var dbName = "the_dotfactory_test";
            var snapshotName = "testdb";
            var snapshotPath = $@"/var/opt/mssql/data/{snapshotName}.ss";
            Common.RemoveTestSnapshot(serverAddr, snapshotName);
            Common.CreateTestDatabase(dbName, serverAddr);
            Common.CreateDatabaseSnapshot(dbName, serverAddr, snapshotName, snapshotPath);
            try
            {
                var contextFactory = new DbContextFactory();
                var repoFactory = new RepositoryFactory(contextFactory, serverAddr, dbName, "SA", "P@ssword", false);
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Common.RestoreDatabaseBySnapshot(dbName, serverAddr, snapshotName);
            }
        }
    }
}