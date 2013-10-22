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

        public BandMembership(int BandId, int MemberId)
        {
            this.BandId = BandId;
            this.MemberId = MemberId;
        }

        [Required]
        [Key]
        [Column(Order=0)]
        public int BandId { get; set; }
        [ForeignKey("BandId")]
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
        public BandDisplayModel() {}

        public BandDisplayModel(string BandName, string CreatorName, string Members)
        {
            this.BandName = BandName;
            this.CreatorName = CreatorName;
            this.Members = Members;
        }

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

    public class BandSearchModel
    {
        [Required]
        [Display(Name = "Band name")]
        public string BandName { get; set; }
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

    public class UpdateBandModel
    {
        [Display(Name = "New Band Name (leave blank to keep current name")]
        public string NewBandName { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password (leave blank to keep current password)")]
        public string NewPassword { get; set; }
    }
}
