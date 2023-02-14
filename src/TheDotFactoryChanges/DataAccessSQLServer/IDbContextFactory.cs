using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccessSQLServer
{
    public interface IDbContextFactory
    {
        IDbContext CreateContext(DbContextOptions options);
    }
}
