using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Domain.Repositories;

namespace Infrastructure.DataAccessSQLServer
{
    public class UserRepository : IUserRepository
    {
        IDbContext _ctx;
        private bool _disposedValue;

        public UserRepository(IDbContext ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException("context");
        }

        public async Task<IEnumerable<Domain.Entities.UserInfo>> GetUserInfos()
        {
            var result = new List<Domain.Entities.UserInfo>();
            IQueryable<UserInfo> users = _ctx.GetUserInfoSet();
            foreach (var info in users.AsParallel())
                result.Add(UserConverter.MapToBusinessEntity(info));
            return result;
        }
        public async Task<Domain.Entities.UserInfo> GetUserById(int id)
        {
            var info = await _ctx.UserInfo.FindAsync(id);
            return info == null ? null : UserConverter.MapToBusinessEntity(info);
        }
        public async Task<Domain.Entities.UserInfo> GetUserByLogin(string login)
        {
            IQueryable<UserInfo> users = _ctx.GetUserInfoSet();
            var info = (from user in users.AsParallel()
                        where user.Usr_login == login
                        select user).FirstOrDefault();
            return info == null ? null : UserConverter.MapToBusinessEntity(info);
        }
        public async Task<int> Create(Domain.Entities.UserInfo info)
        {
            UserInfo dbModel = UserConverter.MapFromBusinessEntity(info);
            _ctx.UserInfo.Add(dbModel);
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch
            {
                throw new ApplicationException("save changes");
            }
            return dbModel.Id;
        }
        public async Task Update(Domain.Entities.UserInfo info)
        {
            _ctx.UserInfo.Update(UserConverter.MapFromBusinessEntity(info));
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch
            {
                throw new ApplicationException("save changes");
            }
        }
        public async Task Delete(int id)
        {
            var info = await _ctx.UserInfo.FindAsync(id);
            if (info == null)
                throw new Exception("No user with such id");

            _ctx.UserInfo.Remove(info);
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch
            {
                throw new ApplicationException("save changes");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _ctx.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}
