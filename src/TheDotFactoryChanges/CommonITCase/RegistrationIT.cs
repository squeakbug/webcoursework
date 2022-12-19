using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using NUnit.Allure.Core;
using NUnit.Framework;

using AuthService;
using DataAccessSQLServer;
using Service;

namespace CommonITCase
{
    [AllureNUnit]
    public class RegistrationIT
    {
        [Test]
        public async Task RegistrationTest()
        {
            string serverAddr = "localhost,1433";
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                serverAddr = "webapp_sqlserver";
            }
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

                var userId = await authService.RegistrateUser("user_001", "password", "password");

                var userRepo = repoFactory.CreateUserRepository();
                var user = await userRepo.GetUserById(userId);
                userRepo.Dispose();
                Assert.NotNull(user);
                Assert.AreEqual(user.Loggined, false);
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
