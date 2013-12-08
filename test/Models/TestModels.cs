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
        [Display(Name = "Name")]
        public string TrackName { get; set; }

        [Display(Name = "Album")]
        public string AlbumName { get; set; }

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
        public string AlbumName { get; set; }
        
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
            this.AlbumName = trackEntry.AlbumName;
            this.TrackUrl = "/Profile/DownloadTrackAudio/" + trackEntry.TrackId;
            this.ImageUrl = "/Profile/DownloadTrackImage/" + trackEntry.TrackId;
        }

        public int TrackId { get; set; }
        public string TrackName { get; set; }
        public string AlbumName { get; set; }
        public string TrackUrl { get; set; }
        public string ImageUrl { get; set; }
    }

    [Table("BandBio")]
    public class BandBio
    {
        public BandBio() {}
        public BandBio(int bandId, string bio)
        {
            this.BandId = bandId;
            this.Bio = bio;
        }

        [Required]
        [Key]
        [Column(Order = 0)]
        public int BandId { get; set; }
        [ForeignKey("BandId")]
        public virtual BandProfile BandProfile { get; set; }

        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }
    }

    public class BandBioModel
    {
        public BandBioModel() {}
        public BandBioModel(string bio)
        {
            this.Bio = bio;
        }

        [DataType(DataType.MultilineText)]
        public string Bio { get; set; }
    }
}

