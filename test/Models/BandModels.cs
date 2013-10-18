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
        public string HashedPassword { get; set; }

        // todo: add other band stuff here
    }

    [Table("BandMembership")]
    public class BandMembership
    {
        // todo: this probably doesn't work

        [Key][Column(Order = 0)][ForeignKey("BandProfile")]
        public int BandId { get; set; }
        [Key][Column(Order = 1)][ForeignKey("UserProfile")]
        public int UserId { get; set; }
    }

    public class RegisterBandModel
    {
        [Required]
        [Display(Name = "Band name")]
        public string BandName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
