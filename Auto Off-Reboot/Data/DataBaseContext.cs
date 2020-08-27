using System.Data.Entity;

namespace Auto_Off_Reboot.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DatabaseConnect") { }

        public DbSet<QuickTiming> QuickTimings { get; set; }
    }
}