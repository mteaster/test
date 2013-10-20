using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Migrations;
using test.Models;
using WebMatrix.WebData;

namespace test.Config
{
    public class MigrationConfiguration : DbMigrationsConfiguration<DatabaseContext>
    {
        public MigrationConfiguration()
        {
            this.AutomaticMigrationsEnabled = true;  // This is important as it will fail in some environments (like Azure) by default
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DatabaseContext context)
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
           
            context.Database.ExecuteSqlCommand("CREATE CLUSTERED INDEX myIndex ON BandMembership (BandName, MemberId)");
        }
    }
}