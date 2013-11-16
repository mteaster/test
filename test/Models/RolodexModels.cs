using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace test.Models.Rolodex
{

    public enum ContactType { Band, People, Venue }

    // This class is used to generalize results from the database for band, people, and venue records
    //      it will be used for the contact list/rolodex
    public class Contact
    {
        [Display(Name = "ContactId")]
        public int ContactId { get; set; }



        [Display(Name = "Type")]
        public ContactType Type { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

    }

    [Table("BandContact")]
    public class BandContact
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }

        [Required]
        public int BandId { get; set; }
        [ForeignKey("BandId")]
        public virtual Band.BandProfile Band { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        // TODO: figure out how to best implement the picture
        [Display(Name = "Picture")]
        public string Picture { get; set; }

        [Display(Name = "Musical Style")]
        public string MusicalStyle { get; set; }

        public int PrimaryPeopleContactId { get; set; }

        public enum Skill { Bad, Poor, OK, Good, Awesome}

        [Display(Name = "Skill Level")]
        public Skill SkillLevel { get; set; }

        public enum Popularity { Unknown, Friends, Moderate, Popular, Famous }

        [Display(Name = "Popularity Level")]
        public Popularity PopularityLevel { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }

    [Table("PeopleContact")]
    public class PeopleContact
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }

        [Required]
        public int BandId { get; set; }
        [ForeignKey("BandId")]
        public virtual Band.BandProfile Band { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        // TODO: figure out how to best implement the picture
        [Display(Name = "Picture")]
        public string Picture { get; set; }

        [Display(Name = "Job/Skill")]
        public string Skill { get; set; }

        [Display(Name="Associated Band")]
        public int BandContactId { get; set; }

        [Display(Name = "Associated Venue")]
        public int VenueContactId { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }

    [Table("VenueContact")]
    public class VenueContact
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ContactId { get; set; }

        [Required]
        public int BandId { get; set; }
        [ForeignKey("BandId")]
        public virtual Band.BandProfile Band { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        // TODO: figure out how to best implement the picture
        [Display(Name = "Picture")]
        public string Picture { get; set; }

        public enum StageSize { Tiny, Small, Average, Big, Huge }

        [Display(Name = "Stage Size")]
        public StageSize StageSizeValue { get; set; }

        public int PrimaryPeopleContactId { get; set; }

        [Display(Name = "Free Beer")]
        public bool FreeBeer { get; set; }

        [Display(Name = "Cover Charge")]
        public bool CoverCharge { get; set; }

        [Display(Name = "Merch Space")]
        public bool MerchSpace { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }
}