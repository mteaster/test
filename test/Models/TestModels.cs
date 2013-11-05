using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models.Test
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

    public class CrazyModel
    {
        public IEnumerable<test.Models.Band.BandModel> UserBands { get; set; }
        public IEnumerable<test.Models.Band.BandModel> AllBands { get; set; }
    }

    public class FileNameModel
    {
        [Required]
        [Display(Name = "File name")]
        public string FileName { get; set; }
    }

    public class FileModel
    {
        [Required]
        [Display(Name = "File")]
        public string FileThing { get; set; }
    }
}