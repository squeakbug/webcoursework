using System.Collections.Generic;

namespace DataAccessInterface
{
    public interface IUserRepository
    {
        IEnumerable<UserInfo> GetUserInfos();
        UserInfo GetUserById(int id);
        UserInfo GetUserByLogin(string login);
        int Create(UserInfo info);
        void Update(UserInfo info);
        void Delete(int id);
    }
}
