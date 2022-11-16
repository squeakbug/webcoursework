using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using NUnit.Allure.Core;
using NUnit.Framework;

using DataAccessSQLServer;
using Service;

namespace CommonITCase
{
    [AllureNUnit]
    public class AddConfigurationIT
    {
        [Test]
        public async Task AddConfigurationTest()
        {
            var snapshotName = "testdb";
            var snapshotPath = $@"/var/opt/mssql/data/{snapshotName}.ss";
            var serverAddr = "webapp_sqlserver";
            var dbName = "the_dotfactory_test";
            Common.RemoveTestSnapshot(serverAddr, snapshotName);
            Common.CreateTestDatabase(dbName, serverAddr);
            Common.CreateDatabaseSnapshot(dbName, serverAddr, snapshotName, snapshotPath);
            try
            {
                var contextFactory = new DbContextFactory();
                var repoFactory = new RepositoryFactory(contextFactory, serverAddr, dbName, "SA", "P@ssword", false);
                var textRenderer = new WinFormsTextRendererAdapter();
                var authService = new AuthService.AuthService(repoFactory);
                var cvtService = new ConverterService(repoFactory, textRenderer);
                await authService.RegistrateUser("user_001", "password", "password");
                await authService.LoginUser("user_001", "password");

                var cfgId = await cvtService.CreateConfig(new DataAccessInterface.Configuration
                {
                    displayName = "displayName",
                });

                var cfgRepo = repoFactory.CreateConfigRepository();
                var cfg = await cfgRepo.GetConfigurationById(cfgId);
                cfgRepo.Dispose();
                Assert.NotNull(cfg);
                Assert.AreEqual(cfg.displayName, "displayName");
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
