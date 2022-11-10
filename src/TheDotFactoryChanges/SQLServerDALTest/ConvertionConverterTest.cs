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
    public class ConvertionConverterTest
    {
        [Test]
        public void MapToBusinessEntityTest()
        {
            var cvtDAL = new DataAccessSQLServer.Convertion
            {
                Id = 1,
                Head = "head_001",
                Body = "body_001",
                Name = "name_001",
            };

            var cvt = ConvertionConverter.MapToBusinessEntity(cvtDAL);

            var targetCvt = new DataAccessInterface.Convertion
            {
                Id = 1,
                Head = "head_001",
                Body = "body_001",
                Name = "name_001",
            };

            Assert.NotNull(cvt);
            Assert.True(cvt.Id.Equals(targetCvt.Id));
            Assert.True(cvt.Head.Equals(targetCvt.Head));
            Assert.True(cvt.Body.Equals(targetCvt.Body));
            Assert.True(cvt.Name.Equals(targetCvt.Name));
        }

        [Test]
        public void MapFromBusinessEntityTest()
        {
            var cvt = new DataAccessInterface.Convertion
            {
                Id = 1,
                Head = "head_001",
                Body = "body_001",
                Name = "name_001",
            };

            var cvtDAL = ConvertionConverter.MapFromBusinessEntity(cvt);

            var targetCvt = new DataAccessSQLServer.Convertion
            {
                Id = 1,
                Head = "head_001",
                Body = "body_001",
                Name = "name_001",
            };

            Assert.NotNull(cvtDAL);
            Assert.True(cvtDAL.Id.Equals(targetCvt.Id));
            Assert.True(cvtDAL.Head.Equals(targetCvt.Head));
            Assert.True(cvtDAL.Body.Equals(targetCvt.Body));
            Assert.True(cvtDAL.Name.Equals(targetCvt.Name));
        }
    }
}
