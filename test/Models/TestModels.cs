using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models
{
    [Table("Log")]
    public class Log
    {
        [Key]
        [Display(Name = "ID")]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        [Required]
        [Display(Name = "Time")]
        public DateTime LogTime { get; set; }

        [Required]
        [Display(Name = "Message")]
        public string LogMessage { get; set; }
    }
}