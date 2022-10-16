using System.Collections.Generic;

using DataAccessInterface;

namespace Presenter
{
    public interface IAuthService
    {
        IEnumerable<UserInfo> GetUsers();
        UserInfo GetUserById(int userId);
        void UpdateUserPassword(int userId, string newPassword);
        void UpdateUserName(int userId, string newName);
        int LoginUser(string login, string password);
        void LogoutUser(int userId);
        int RegistrateUser(string login, string password, string repPassword);
        bool IsUserLoggined(int userId);
    }
}
