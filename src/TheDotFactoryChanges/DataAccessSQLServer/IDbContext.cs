using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace DataAccessSQLServer
{
    public interface IDbContext
    {
        DbSet<UserInfo> UserInfo { get; set; }
        DbSet<UserConfig> UserConfig { get; set; }
        DbSet<Convertion> Convertions { get; set; }
        DbSet<Font> Fonts { get; set; }

        IQueryable<UserInfo> GetUserInfoSet();
        IQueryable<UserConfig> GetUserConfigSet();
        IQueryable<Convertion> GetConvertionSet();
        IQueryable<Font> GetFontSet();

        void DatabaseEnsureCreated();
        int SaveChanges();
    }
}
