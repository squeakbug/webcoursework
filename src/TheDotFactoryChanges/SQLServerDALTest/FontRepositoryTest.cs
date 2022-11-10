using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using NUnit.Allure.Core;
using Microsoft.EntityFrameworkCore;
using Moq;

using DataAccessSQLServer;

namespace SQLServerDALTest
{
    [AllureNUnit]
    public class FontRepositoryTest
    {
        [Test]
        public void CreateFontDefaultTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var fontDbSetMock = new Mock<DbSet<DataAccessSQLServer.Font>>();
            var fontSetMock = new Mock<Context>(options);
            fontSetMock.Setup(f => f.Fonts)
                       .Returns(fontDbSetMock.Object);
            fontSetMock.Setup(set =>
                set.Fonts.Add(It.IsAny<DataAccessSQLServer.Font>()));
            fontSetMock.Setup(set => set.SaveChangesAsync());
            var sut = new FontRepository(fontSetMock.Object);
            var font = GetFontSample()[0];

            sut.Create(font);

            fontSetMock.Verify(set =>
                set.Fonts.Add(It.Is<DataAccessSQLServer.Font>(f =>
                    f.Name == font.Name)));
            fontSetMock.Verify(set => set.SaveChangesAsync());
        }

        [Test]
        public async Task GetFontByIdTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var fontDbSetMock = new Mock<DbSet<DataAccessSQLServer.Font>>();
            var fontSetMock = new Mock<Context>(options);
            var font = GetDALFontSample()[0];
            fontSetMock.Setup(f => f.Fonts)
                       .Returns(fontDbSetMock.Object);
            fontSetMock.Setup(set =>
                set.Fonts.FindAsync(It.IsAny<int>()))
                       .ReturnsAsync(font);
            var sut = new FontRepository(fontSetMock.Object);

            var retFont = await sut.GetFontById(font.Id);

            fontSetMock.Verify(set =>
                set.Fonts.FindAsync(It.Is<int>(id =>
                    id == font.Id)));
        }

        [Test]
        public void GetFontsTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var fontDbSetMock = new Mock<DbSet<DataAccessSQLServer.Font>>();
            var fontSetMock = new Mock<Context>(options);
            fontSetMock.Setup(f => f.Fonts)
                       .Returns(fontDbSetMock.Object);
            var fonts = GetDALFontSample();
            fontSetMock.Setup(f => f.GetFontSet())
                       .Returns(fonts.AsQueryable());
            var sut = new FontRepository(fontSetMock.Object);

            var retFont = sut.GetFonts();

            fontSetMock.Verify(set => set.GetFontSet());
        }

        [Test]
        public void UpdateFontTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var fontDbSetMock = new Mock<DbSet<DataAccessSQLServer.Font>>();
            var fontSetMock = new Mock<Context>(options);
            fontSetMock.Setup(f => f.Fonts)
                       .Returns(fontDbSetMock.Object);
            var font = GetFontSample()[0];
            var sut = new FontRepository(fontSetMock.Object);

            sut.Update(font);

            fontSetMock.Verify(set => set.Fonts);
        }

        [Test]
        public void DeleteFontTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var fontDbSetMock = new Mock<DbSet<DataAccessSQLServer.Font>>();
            var fontSetMock = new Mock<Context>(options);
            var font = GetDALFontSample()[0];
            fontSetMock.Setup(u => u.Fonts)
                       .Returns(fontDbSetMock.Object);
            fontSetMock.Setup(set =>
                set.Fonts.Find(It.IsAny<int>()))
                       .Returns(font);
            fontSetMock.Setup(set =>
                set.Fonts.Remove(It.IsAny<DataAccessSQLServer.Font>()));
            var sut = new FontRepository(fontSetMock.Object);

            sut.Delete(font.Id);

            fontSetMock.Verify(set =>
                set.Fonts.Remove(It.IsAny<DataAccessSQLServer.Font>()));
        }

        private List<DataAccessSQLServer.Font> GetDALFontSample()
        {
            return new List<DataAccessSQLServer.Font>()
            {
                new DataAccessSQLServer.Font
                {
                    Id = 1,
                    Name = "name_001",
                    Size = 10,
                },
                new DataAccessSQLServer.Font
                {
                    Id = 2,
                    Name = "name_002",
                    Size = 10,
                },
                new DataAccessSQLServer.Font
                {
                    Id = 3,
                    Name = "name_003",
                    Size = 10,
                }
            };
        }
        private List<DataAccessInterface.Font> GetFontSample()
        {
            return new List<DataAccessInterface.Font>()
            {
                new DataAccessInterface.Font
                {
                    Id = 1,
                    Name = "name_001",
                    Size = 10,
                },
                new DataAccessInterface.Font
                {
                    Id = 2,
                    Name = "name_002",
                    Size = 10,
                },
                new DataAccessInterface.Font
                {
                    Id = 3,
                    Name = "name_003",
                    Size = 10,
                }
            };
        }
    }
}
