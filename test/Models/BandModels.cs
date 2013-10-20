using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.Mvc;

namespace test.Models
{
    [Table("BandProfile")]
    public class BandProfile
    {
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

    public class BandDisplayModel
    {
        [Required]
        public string BandName;

        [Required]
        public string CreatorName;
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
