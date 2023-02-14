using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository : IDisposable
    {
        Task<IEnumerable<UserInfo>> GetUserInfos();
        Task<UserInfo> GetUserById(int id);
        Task<UserInfo> GetUserByLogin(string login);
        Task<int> Create(UserInfo info);
        Task Update(UserInfo info);
        Task Delete(int id);
    }
}
