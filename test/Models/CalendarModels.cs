using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.Mvc;
using System.Collections.Generic;
using System;

namespace test.Models
{
    [Table("CalendarEvent")]
    public class CalendarEvent
    {
        public CalendarEvent() {}

        [Key]
        [Required]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        [Required]
        public string EventTitle { get; set; }

        [Required]
        public int BandId { get; set; }
        [ForeignKey("BandId")]
        public virtual BandProfile BandProfile { get; set; }

        [Required]
        public DateTime EventTime { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string EventDescription { get; set; }
    }

    public class CalendarEventModel
    {
        public CalendarEventModel() {}

        [Required]
        [Display(Name = "Title")]
        public string EventTitle { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string EventDescription { get; set; }

        [Required]
        [Display(Name = "Month (Number)")]
        public int EventMonth { get; set; }

        [Required]
        [Display(Name = "Day (Number)")]
        public int EventDay { get; set; }

        [Required]
        [Display(Name = "Year (Number)")]
        public int EventYear { get; set; }
    }


}