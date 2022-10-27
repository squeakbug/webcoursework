using DataAccessSQLServer;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Allure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerDALTest
{
    [AllureNUnit]
    public class ConvertionRepositoryTest
    {
        [Test]
        public void CreateConvertionTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var cvtDbSetMock = new Mock<DbSet<DataAccessSQLServer.Convertion>>();
            var cvtSetMock = new Mock<Context>(options);
            cvtSetMock.Setup(c => c.Convertions)
                      .Returns(cvtDbSetMock.Object);
            cvtSetMock.Setup(set =>
                set.Convertions.Add(It.IsAny<DataAccessSQLServer.Convertion>()));
            cvtSetMock.Setup(set =>
                set.SaveChanges());
            var sut = new ConvertionRepository(cvtSetMock.Object);
            var cvt = GetConvertionSample()[0];

            sut.Create(cvt);

            cvtSetMock.Verify(set =>
                set.Convertions.Add(It.Is<DataAccessSQLServer.Convertion>(cvt =>
                    cvt.Name == cvt.Name)));
            cvtSetMock.Verify(set =>
                set.SaveChanges());
        }

        [Test]
        public void GetConvertionByIdTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var cvtDbSetMock = new Mock<DbSet<DataAccessSQLServer.Convertion>>();
            var cvtSetMock = new Mock<Context>(options);
            var cvt = GetDALConvertionSample()[0];
            cvtSetMock.Setup(c => c.Convertions)
                       .Returns(cvtDbSetMock.Object);
            cvtSetMock.Setup(set =>
                set.Convertions.Find(It.IsAny<int>()))
                       .Returns(cvt);
            var sut = new ConvertionRepository(cvtSetMock.Object);

            var retCvt = sut.GetConvertionById(cvt.Id);

            cvtSetMock.Verify(set =>
                set.Convertions.Find(It.Is<int>(id =>
                    id == cvt.Id)));
        }

        [Test]
        public void GetConvertionsTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var cvtDbSetMock = new Mock<DbSet<DataAccessSQLServer.Convertion>>();
            var cvtSetMock = new Mock<Context>(options);
            cvtSetMock.Setup(c => c.Convertions)
                       .Returns(cvtDbSetMock.Object);
            var cvts = GetDALConvertionSample();
            cvtSetMock.Setup(c => c.GetConvertionSet())
                       .Returns(cvts.AsQueryable());
            var sut = new ConvertionRepository(cvtSetMock.Object);

            var retCvts = sut.GetConvertions();

            cvtSetMock.Verify(set => set.GetConvertionSet());
            Assert.NotNull(retCvts);
            Assert.AreEqual(3, retCvts.Count());
        }

        [Test]
        public void UpdateConvertionTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var cvtDbSetMock = new Mock<DbSet<DataAccessSQLServer.Convertion>>();
            var cvtSetMock = new Mock<Context>(options);
            cvtSetMock.Setup(c => c.Convertions)
                       .Returns(cvtDbSetMock.Object);
            var cvt = GetConvertionSample()[0];
            var sut = new ConvertionRepository(cvtSetMock.Object);

            sut.Update(cvt);

            cvtSetMock.Verify(set => set.Convertions);
        }

        [Test]
        public void DeleteConvertionTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var cvtDbSetMock = new Mock<DbSet<DataAccessSQLServer.Convertion>>();
            var cvtSetMock = new Mock<Context>(options);
            var cvt = GetDALConvertionSample()[0];
            cvtSetMock.Setup(c => c.Convertions)
                      .Returns(cvtDbSetMock.Object);
            cvtSetMock.Setup(set =>
                set.Convertions.Find(It.IsAny<int>()))
                      .Returns(cvt);
            cvtSetMock.Setup(set =>
                set.Convertions.Remove(It.IsAny<DataAccessSQLServer.Convertion>()));
            var sut = new ConvertionRepository(cvtSetMock.Object);

            sut.Delete(cvt.Id);

            cvtSetMock.Verify(set =>
                set.Convertions.Remove(It.IsAny<DataAccessSQLServer.Convertion>()));
        }


        private List<DataAccessSQLServer.Convertion> GetDALConvertionSample()
        {
            return new List<DataAccessSQLServer.Convertion>()
            {
                new DataAccessSQLServer.Convertion
                {
                    Id = 1,
                    Body = "body_001",
                    Head = "head_001",
                    Name = "name_001",
                },
                new DataAccessSQLServer.Convertion
                {
                    Id = 2,
                    Body = "body_002",
                    Head = "head_002",
                    Name = "name_002",
                },
                new DataAccessSQLServer.Convertion
                {
                    Id = 3,
                    Body = "body_003",
                    Head = "head_003",
                    Name = "name_003",
                }
            };
        }

        private List<DataAccessInterface.Convertion> GetConvertionSample()
        {
            return new List<DataAccessInterface.Convertion>()
            {
                new DataAccessInterface.Convertion
                {
                    Id = 1,
                    Body = "body_001",
                    Head = "head_001",
                    Name = "name_001",
                },
                new DataAccessInterface.Convertion
                {
                    Id = 2,
                    Body = "body_002",
                    Head = "head_002",
                    Name = "name_002",
                },
                new DataAccessInterface.Convertion
                {
                    Id = 3,
                    Body = "body_003",
                    Head = "head_003",
                    Name = "name_003",
                }
            };
        }
    }
}
