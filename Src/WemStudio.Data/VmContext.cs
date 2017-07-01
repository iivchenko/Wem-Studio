using System.Data.Entity;

namespace WemManagementStudio.Data
{
    public sealed class VMContext : DbContext
    {
        public DbSet<Machine> Machines { get; set; }
    }
}
