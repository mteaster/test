using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.Mvc;

namespace test.Models
{
    [Table("BandProfile")]
    public class BandProfile
    {
        public BandProfile() {}
        public BandProfile(string BandName, int CreatorId, string Password)
        {
            this.BandName = BandName;
            this.CreatorId = CreatorId;
            this.Password = Password;
        }
        
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int BandId { get; set; }

        [Required]
        public string BandName { get; set; }

        [Required]
        public int CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual UserProfile CreatorProfile { get; set; }

        [Required]
        public string Password { get; set; }
    }

    [Table("BandMembership")]
    public class BandMembership
    {
        public BandMembership() {}

        public BandMembership(string BandName, int MemberId)
        {
            this.BandName = BandName;
            this.MemberId = MemberId;
        }

        [Required]
        [Key]
        [Column(Order=0)]
        public string BandName { get; set; }
        [ForeignKey("BandName")]
        public virtual BandProfile BandProfile { get; set; }

        [Required]
        [Key]
        [Column(Order = 1)]
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual UserProfile MemberProfile { get; set; }
    }

    public class BandDisplayModel
    {
        [Required]
        [Display(Name = "Band")]
        public string BandName { get; set; }

        [Required]
        [Display(Name = "Creator")]
        public string CreatorName { get; set; }

        [Required]
        [Display(Name = "Members")]
        public string Members { get; set; }
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
