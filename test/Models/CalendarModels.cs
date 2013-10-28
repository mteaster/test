﻿using System.ComponentModel.DataAnnotations;
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
        public DateTime EventTime { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string EventDescription { get; set; }
    }

    public class CalendarEventModel
    {
        public CalendarEventModel() {}

        [Required]
        public string EventTitle { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string EventDescription { get; set; }

        [Required]
        public int EventMonth { get; set; }

        [Required]
        public int EventDay { get; set; }

        [Required]
        public int EventYear { get; set; }
    }


}