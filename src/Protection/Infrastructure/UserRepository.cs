using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private Context _ctx;
        private bool _disposedValue;

        public UserRepository(Context ctx)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(Context));
        }

        public async Task<IEnumerable<Domain.User>> GetUsers()
        {
            var result = new List<Domain.User>();
            foreach (var info in _ctx.Users)
                result.Add(UserConverter.MapToBusinessEntity(info));
            return result;
        }
        public async Task<Domain.User?> GetUserById(Guid id)
        {
            var user = await _ctx.Users.FindAsync(id);
            return user == null ? null : UserConverter.MapToBusinessEntity(user);
        }
        public async Task<Domain.User?> Create(Domain.NewUser newUser)
        {
            User userdb = UserConverter.MapFromBusinessEntity(newUser);
            _ctx.Users.Add(userdb);
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch
            {
                return null;
            }
            return UserConverter.MapToBusinessEntity(userdb);
        }
        public async Task<Domain.User?> Update(Domain.User user)
        {
            var userdb = UserConverter.MapFromBusinessEntity(user);
            _ctx.Users.Update(userdb);
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch
            {
                return null;
            }
            return UserConverter.MapToBusinessEntity(userdb);
        }
        public async Task<bool> Delete(Guid id)
        {
            var user = await _ctx.Users.FindAsync(id);
            if (user == null)
                return false;

            _ctx.Users.Remove(user);
            try
            {
                await _ctx.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
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
