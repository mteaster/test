using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace test.Models
{
    public class BandsContext : DbContext
    {
        public BandsContext() : base("DefaultConnection")
        {
        }

        public DbSet<BandProfile> BandProfiles { get; set; }
    }

    [Table("BandProfile")]
    public class BandProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BandId { get; set; }
        public string BandName { get; set; }
    }
}
