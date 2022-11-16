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

                var fontId = await cvtService.AddFont(new DataAccessInterface.Font()
                {
                    Name = "DejaVu Sans",
                    Size = 10,
                });

                var fontRepo = repoFactory.CreateFontRepository();
                var font = await fontRepo.GetFontById(fontId);
                fontRepo.Dispose();
                Assert.NotNull(font);
                Assert.AreEqual(font.Name, "DejaVu Sans");
                Assert.AreEqual(font.Size, 10);
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
