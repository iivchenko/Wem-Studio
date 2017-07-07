using System.Data.Entity;
using WemStudio.Domain;

namespace WemStudio.Data
{
    public sealed class WemStudioContext : DbContext
    {
        public WemStudioContext() 
            : base("WemStudio")
        {
        }

        public DbSet<Machine> Machines { get; set; }
    }
}
