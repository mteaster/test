using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.Mvc;
using System.Collections.Generic;
using System;

namespace test.Models
{
    [Table("Event")]
    public class Event
    {
        public Event() {}

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
}