using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Allure.Core;
using NUnit.Framework;

using DataAccessInterface;
using DataAccessSQLServer;

namespace SQLServerDALTest
{
    [AllureNUnit]
    public class UserConverterTest
    {
        [Test]
        public void MapToBusinessEntityTest()
        {
            var userDAL = new DataAccessSQLServer.UserInfo
            {
                Id = 1,
                Usr_Loggined = true,
                Usr_login = "user_001",
                Usr_name = "name_001",
                Usr_passwd = "pass_001",
            };

            var user = UserConverter.MapToBusinessEntity(userDAL);

            var targetUser = new DataAccessInterface.UserInfo
            {
                Id = 1,
                Loggined = true,
                Login = "user_001",
                Name = "name_001",
                Password = "pass_001",
            };
            Assert.NotNull(user);
            Assert.That(user.Id.Equals(targetUser.Id));
            Assert.That(user.Login.Equals(targetUser.Login));
            Assert.That(user.Name.Equals(targetUser.Name));
            Assert.That(user.Password.Equals(targetUser.Password));
        }

        [Test]
        public void MapFromBusinessEntityTest()
        {
            var user = new DataAccessInterface.UserInfo
            {
                Id = 1,
                Loggined = true,
                Login = "user_001",
                Name = "name_001",
                Password = "pass_001",
            };

            var userDAL = UserConverter.MapFromBusinessEntity(user);

            var targetUser = new DataAccessSQLServer.UserInfo
            {
                Id = 1,
                Usr_Loggined = true,
                Usr_login = "user_001",
                Usr_name = "name_001",
                Usr_passwd = "pass_001",
            };
            Assert.NotNull(userDAL);
            Assert.That(userDAL.Id.Equals(targetUser.Id));
            Assert.That(userDAL.Usr_Loggined.Equals(targetUser.Usr_Loggined));
            Assert.That(userDAL.Usr_name.Equals(targetUser.Usr_name));
            Assert.That(userDAL.Usr_passwd.Equals(targetUser.Usr_passwd));
        }
    }
}
