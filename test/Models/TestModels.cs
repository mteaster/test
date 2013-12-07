using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using test.Models.Band;

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

    public class UploadTrackModel
    {
        [Display(Name = "Track name")]
        public string TrackName { get; set; }

        [Required]
        [Display(Name = "Audio file")]
        public HttpPostedFileBase TrackAudio { get; set; }

        [Display(Name = "Image file")]
        public HttpPostedFileBase TrackImage { get; set; }
    }

    public class TrackEntry
    {
        public TrackEntry() {}
        public TrackEntry(string trackName, int bandId)
        {
            this.TrackName = trackName;
            this.BandId = bandId;
        }

        [Key]
        [Required]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int TrackId { get; set; }
        
        [Required]
        public string TrackName { get; set; }
        
        [Required]
        public int BandId { get; set; }
        [ForeignKey("BandId")]
        public virtual BandProfile BandProfile { get; set; }
    }

    public class TrackEntryModel
    {
        public TrackEntryModel() { }
        public TrackEntryModel(TrackEntry trackEntry)
        {
            this.TrackName = trackEntry.TrackName;
            this.TrackId = trackEntry.TrackId;
            this.TrackUrl = "/Online/DownloadTrackAudio/" + trackEntry.TrackId;
            this.ImageUrl = "/Online/DownloadTrackImage/" + trackEntry.TrackId;
        }

        public int TrackId { get; set; }
        public string TrackName { get; set; }
        public string TrackUrl { get; set; }
        public string ImageUrl { get; set; }
    }
}

