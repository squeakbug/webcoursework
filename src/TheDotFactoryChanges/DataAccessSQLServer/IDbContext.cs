using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccessSQLServer
{
    public interface IDbContext
    {
        DbSet<UserInfo> UserInfo { get; set; }
        DbSet<Convertion> Convertions { get; set; }
        DbSet<Font> Fonts { get; set; }

        IQueryable<UserInfo> GetUserInfoSet();
        IQueryable<Convertion> GetConvertionSet();
        IQueryable<Font> GetFontSet();

        void DatabaseEnsureCreated();
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void Dispose();
    }
}
