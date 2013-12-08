using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using test.Models.Account;
using test.Models.Band;
using test.Models.Calendar;
using test.Models.Dashboard;
using test.Models.FileCabinet;
using test.Models.Rolodex;
using test.Models.Test;
using test.Models.Budget;

namespace test.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DefaultConnection") {}

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<BandProfile> BandProfiles { get; set; }
        public DbSet<BandMembership> BandMemberships { get; set; }
        public DbSet<MessageBoardPost> MessageBoardPosts { get; set; }
        public DbSet<CalendarEvent> CalendarEvents { get; set; }
        public DbSet<FileEntry> FileEntries { get; set; }
        public DbSet<FileGroup> FileGroups { get; set; }
        public DbSet<BandContact> BandContacts { get; set; }
        public DbSet<PeopleContact> PeopleContacts { get; set; }
        public DbSet<VenueContact> VenueContacts { get; set; }
        public DbSet<AccountPayables> AccountPayables { get; set; }
        public DbSet<AccountReceivables> AccountReceivables { get; set; }
        public DbSet<Merchandise> Merchandise { get; set; }
        public DbSet<BandBio> BandBios { get; set; }

        public DbSet<TrackEntry> TrackEntries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();   

            base.OnModelCreating(modelBuilder);
        }
    }
}