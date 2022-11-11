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
    public class AddFontIT
    {
        [Test]
        public async Task AddFontTest()
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

            var fontId = await cvtService.AddFont(new DataAccessInterface.Font()
            {
                Name = "Arial",
                Size = 10,
            });

            var fontRepo = repoFactory.CreateFontRepository();
            var font = await fontRepo.GetFontById(fontId);
            fontRepo.Dispose();
            Assert.NotNull(font);
            Assert.AreEqual(font.Name, "Arial");
            Assert.AreEqual(font.Size, 10);
            Common.RestoreDatabaseBySnapshot("the_dotfactory_test", "127.0.0.1", snapshotName);
        }
    }
}
