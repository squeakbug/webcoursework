using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Allure.Core;
using NUnit.Framework;

using DataAccessSQLServer;

namespace SQLServerDALTest
{
    [AllureNUnit]
    public class FontConverterTest
    {
        [Test]
        public void MapToBusinessEntityTest()
        {
            var fontDAL = new DataAccessSQLServer.Font
            {
                Id = 1,
                Name = "name_001",
                Size = 10,
            };

            var font = FontConverter.MapToBusinessEntity(fontDAL);

            var targetFont = new DataAccessInterface.Font
            {
                Id = 1,
                Name = "name_001",
                Size = 10,
            };
            Assert.NotNull(font);
            Assert.True(font.Id.Equals(targetFont.Id));
            Assert.True(font.Name.Equals(targetFont.Name));
            Assert.True(font.Size.Equals(targetFont.Size));
        }

        [Test]
        public void MapFromBusinessEntityTest()
        {
            var font = new DataAccessInterface.Font
            {
                Id = 1,
                Name = "name_001",
                Size = 10,
            };

            var fontDAL = FontConverter.MapFromBusinessEntity(font);

            var targetFont = new DataAccessSQLServer.Font
            {
                Id = 1,
                Name = "name_001",
                Size = 10,
            };
            Assert.NotNull(fontDAL);
            Assert.True(fontDAL.Id.Equals(targetFont.Id));
            Assert.True(fontDAL.Name.Equals(targetFont.Name));
            Assert.True(fontDAL.Size.Equals(targetFont.Size));
        }
    }
}
