using DataAccessSQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDALTest
{
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
            Assert.AreEqual(targetCvt.Name, cvt.Name);
            Assert.AreEqual(targetCvt.Id, cvt.Id);
            Assert.AreEqual(targetCvt.Body, cvt.Body);
            Assert.AreEqual(targetCvt.Head, cvt.Head);
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
            Assert.AreEqual(targetCvt.Name, cvtDAL.Name);
            Assert.AreEqual(targetCvt.Id, cvtDAL.Id);
            Assert.AreEqual(targetCvt.Body, cvtDAL.Body);
            Assert.AreEqual(targetCvt.Head, cvtDAL.Head);
        }
    }
}
