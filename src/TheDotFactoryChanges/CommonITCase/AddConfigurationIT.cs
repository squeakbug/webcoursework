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
            Common.CreateTestDatabase("the_dotfactory_test", "127.0.0.1");
            var snapshotName = "testdb";
            var snapshotPath = $@"/var/opt/mssql/data/{snapshotName}.ss";
            Common.CreateDatabaseSnapshot("the_dotfactory_test", "127.0.0.1", snapshotName, snapshotPath);
            var contextFactory = new DbContextFactory();
            var repoFactory = new RepositoryFactory(contextFactory, "127.0.0.1", "the_dotfactory_test", "SA", "P@ssword", false);
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
            Common.RestoreDatabaseBySnapshot("the_dotfactory_test", "127.0.0.1", snapshotName);
        }
    }
}
