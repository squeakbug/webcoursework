using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessSQLServer
{
    public class DbContextFactory : IDbContextFactory
    {
        public IDbContext CreateContext(DbContextOptions options)
        {
            return new Context(options);
        }
    }
}
