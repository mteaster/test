﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using test.Models.Band;
using System.Collections.Generic;

namespace test.Models.Calendar
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

    public class MonthModel
    {
        public int DaysInMonth { get; set; }
        public string MonthName { get; set; }

        public int CurrentMonth { get; set; }
        public int CurrentMonthYear { get; set; }

        public int PreviousMonth { get; set; }
        public int PreviousMonthYear { get; set; }

        public int NextMonth { get; set; }
        public int NextMonthYear { get; set; }

        public IEnumerable<test.Models.Calendar.CalendarEvent> Events;
        

    }


}