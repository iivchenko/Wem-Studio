using System.Data.Entity;
using WemStudio.Domain;

namespace WemStudio.Data
{
    public sealed class VMContext : DbContext
    {
        public DbSet<Machine> Machines { get; set; }
    }
}
