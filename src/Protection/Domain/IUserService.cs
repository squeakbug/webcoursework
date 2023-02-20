using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User?> GetUserById(Guid id);
        Task<User?> Create(NewUser newUser);
        Task<User?> Update(User user);
        Task<bool> Delete(Guid id);
    }
}
