using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccessSQLServer
{
    public class Context : DbContext, IDbContext
    {
        public Context(DbContextOptions options)
            : base(options)
        { }

        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<Convertion> Convertions { get; set; }
        public virtual DbSet<Font> Fonts { get; set; }

        public virtual IQueryable<UserInfo> GetUserInfoSet()
        {
            base.Database.EnsureCreated();
            return UserInfo;
        }

        public virtual IQueryable<Convertion> GetConvertionSet()
        {
            return Convertions;
        }

        public virtual IQueryable<Font> GetFontSet()
        {
            return Fonts;
        }

        public void DatabaseEnsureCreated()
        {
            Database.EnsureCreated();
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
