using System;
using System.Collections.Generic;
using System.Linq;

using DataAccessInterface;

namespace DataAccessSQLServer
{
    public class UserRepository : IUserRepository
    {
        Context _ctx;

        public UserRepository(Context ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException("context");
        }

        public IEnumerable<DataAccessInterface.UserInfo> GetUserInfos()
        {
            var result = new List<DataAccessInterface.UserInfo>();
            IQueryable<UserInfo> users = _ctx.GetUserInfoSet();
            foreach (var info in users)
                result.Add(UserConverter.MapToBusinessEntity(info));
            return result;
        }
        public DataAccessInterface.UserInfo GetUserById(int id)
        {
            var info = _ctx.UserInfo.Find(id);
            return info == null ? null : UserConverter.MapToBusinessEntity(info);
        }
        public DataAccessInterface.UserInfo GetUserByLogin(string login)
        {
            IQueryable<UserInfo> users = _ctx.GetUserInfoSet();
            var info = (from user in users
                        where user.Usr_login == login
                        select user).FirstOrDefault();
            return info == null ? null : UserConverter.MapToBusinessEntity(info);
        }
        public int Create(DataAccessInterface.UserInfo info)
        {
            UserInfo dbModel = UserConverter.MapFromBusinessEntity(info);
            _ctx.UserInfo.Add(dbModel);
            _ctx.SaveChanges();
            return dbModel.Id;
        }
        public void Update(DataAccessInterface.UserInfo info)
        {
            _ctx.UserInfo.Update(UserConverter.MapFromBusinessEntity(info));
            _ctx.SaveChanges();
        }
        public void Delete(int id)
        {
            var info = _ctx.UserInfo.Find(id);
            if (info == null)
                throw new Exception("No user with such id");

            _ctx.UserInfo.Remove(info);
            _ctx.SaveChanges();
        }
    }
}
