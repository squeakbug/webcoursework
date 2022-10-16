using DataAccessInterface;
using DataAccessSQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlServerDALTest
{
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
            Assert.AreEqual(user.Id, targetUser.Id);
            Assert.AreEqual(user.Password, targetUser.Password);
            Assert.AreEqual(user.Login, targetUser.Login);
            Assert.AreEqual(user.Name, targetUser.Name);
            Assert.AreEqual(user.Loggined, targetUser.Loggined);
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
            Assert.AreEqual(userDAL.Id, targetUser.Id);
            Assert.AreEqual(userDAL.Usr_passwd, targetUser.Usr_passwd);
            Assert.AreEqual(userDAL.Usr_login, targetUser.Usr_login);
            Assert.AreEqual(userDAL.Usr_name, targetUser.Usr_name);
            Assert.AreEqual(userDAL.Usr_Loggined, targetUser.Usr_Loggined);
        }
    }
}
