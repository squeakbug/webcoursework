using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataAccessSQLServer
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options)
            : base(options)
        { }

        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<UserConfig> UserConfig { get; set; }
        public virtual DbSet<Convertion> Convertions { get; set; }
        public virtual DbSet<Font> Fonts { get; set; }

        public virtual IQueryable<UserInfo> GetUserInfoSet()
        {
            return UserInfo;
        }

        public virtual IQueryable<UserConfig> GetUserConfigSet()
        {
            return UserConfig;
        }

        public virtual IQueryable<Convertion> GetConvertionSet()
        {
            return Convertions;
        }

        public virtual IQueryable<Font> GetFontSet()
        {
            return Fonts;
        }
    }
}
