using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Allure.Core;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Moq;

using DataAccessSQLServer;
using Microsoft.Data.SqlClient;

namespace SQLServerDALTest
{
    [AllureNUnit]
    public class RepositoryFactoryTest
    {
        [Test]
        public void CreateUserRepositoryTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var contextMock = new Mock<IDbContext>();
            contextMock.Setup(f => f.DatabaseEnsureCreated());
            var ctxFactoryMock = new Mock<IDbContextFactory>();
            ctxFactoryMock.Setup(f => f.CreateContext(It.IsAny<DbContextOptions>()))
                          .Returns(contextMock.Object);
            var sut = new RepositoryFactory(ctxFactoryMock.Object, "127.0.0.1", "the_dotfactory", "SA", "P@ssword");

            var repo = sut.CreateUserRepository();

            contextMock.Verify(c => c.DatabaseEnsureCreated());
        }

        [Test]
        public void CreateConfigRepositoryTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummyString")
                .Options;
            var contextMock = new Mock<IDbContext>();
            contextMock.Setup(f => f.DatabaseEnsureCreated());
            var ctxFactoryMock = new Mock<IDbContextFactory>();
            ctxFactoryMock.Setup(f => f.CreateContext(It.IsAny<DbContextOptions>()))
                          .Returns(contextMock.Object);
            var sut = new RepositoryFactory(ctxFactoryMock.Object, "127.0.0.1", "the_dotfactory", "SA", "P@ssword", false);

            var repo = sut.CreateConfigRepository();

            contextMock.Verify(c => c.DatabaseEnsureCreated());
        }

        [Test]
        public void CreateConvertionRepositoryTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var contextMock = new Mock<IDbContext>();
            contextMock.Setup(f => f.DatabaseEnsureCreated());
            var ctxFactoryMock = new Mock<IDbContextFactory>();
            ctxFactoryMock.Setup(f => f.CreateContext(It.IsAny<DbContextOptions>()))
                          .Returns(contextMock.Object);
            var sut = new RepositoryFactory(ctxFactoryMock.Object, "127.0.0.1", "the_dotfactory", "SA", "P@ssword");

            var repo = sut.CreateConvertionRepository();

            contextMock.Verify(c => c.DatabaseEnsureCreated());
        }

        [Test]
        public void CreateFontRepositoryTest()
        {
            var options = SqlServerDbContextOptionsExtensions
                .UseSqlServer(new DbContextOptionsBuilder(), "dummy_string")
                .Options;
            var contextMock = new Mock<IDbContext>();
            contextMock.Setup(f => f.DatabaseEnsureCreated());
            var ctxFactoryMock = new Mock<IDbContextFactory>();
            ctxFactoryMock.Setup(f => f.CreateContext(It.IsAny<DbContextOptions>()))
                          .Returns(contextMock.Object);
            var sut = new RepositoryFactory(ctxFactoryMock.Object, "127.0.0.1", "the_dotfactory", "SA", "P@ssword");

            var repo = sut.CreateFontRepository();

            contextMock.Verify(c => c.DatabaseEnsureCreated());
        }
    }
}
