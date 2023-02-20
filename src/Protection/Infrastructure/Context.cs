using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options)
            : base(options)
        { }

        public virtual DbSet<User> Users { get; set; }
    }
}