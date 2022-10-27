using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessSQLServer
{
    public interface IDbContextFactory
    {
        IDbContext CreateContext(DbContextOptions options);
    }
}
