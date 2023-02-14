using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccessSQLServer
{
    public class DbContextFactory : IDbContextFactory
    {
        public IDbContext CreateContext(DbContextOptions options)
        {
            return new Context(options);
        }
    }
}
