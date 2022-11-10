using System.Collections.Generic;
using System.Threading.Tasks;

using DataAccessInterface;

namespace Presenter
{
    public interface IAuthService
    {
        Task<IEnumerable<UserInfo>> GetUsers();
        Task<UserInfo> GetUserById(int userId);
        Task UpdateUserPassword(int userId, string newPassword);
        Task UpdateUserName(int userId, string newName);
        Task<int> LoginUser(string login, string password);
        Task LogoutUser(int userId);
        Task<int> RegistrateUser(string login, string password, string repPassword);
        Task<bool> IsUserLoggined(int userId);
    }
}
