using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace test.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<BandProfile> BandProfiles { get; set; }
        public DbSet<BandMembership> BandMemberships { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}