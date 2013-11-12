using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using test.Models.Account;
using test.Models.Band;

namespace test.Models.FileCabinet
{
    [Table("FileEntry")]
    public class FileEntry
    {
        public FileEntry() {}
        public FileEntry(string fileName, int bandId, int groupId, int uploaderId)
        {
            this.FileName = fileName;
            this.GroupId = groupId;
            this.BandId = bandId;
            this.UploaderId = uploaderId;
        }

        [Key]
        [Required]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FileId { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public int BandId { get; set; }
        [ForeignKey("BandId")]
        public virtual BandProfile BandProfile { get; set; }

        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual FileGroup FileGroup { get; set; }

        [Required]
        public int UploaderId { get; set; }
        [ForeignKey("UploaderId")]
        public virtual UserProfile UploaderProfile { get; set; }
    }

    [Table("FileGroup")]
    public class FileGroup
    {
        [Key]
        [Required]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int GroupId { get; set; }

        [Required]
        public string GroupName { get; set; }

        [Required]
        public int BandId { get; set; }
        [ForeignKey("BandId")]
        public virtual BandProfile BandProfile { get; set; }
    }

    public class FileEntryModel
    {
        public FileEntryModel() { }
        public FileEntryModel(int fileId, string fileName, int bandId, int groupId, int uploaderId)
        {
            this.FileId = fileId;
            this.FileName = fileName;
            this.GroupId = groupId;
            this.BandId = bandId;
            this.UploaderId = uploaderId;
        }

        public int FileId { get; set; }
        public string FileName { get; set; }
        public int BandId { get; set; }
        public int GroupId { get; set; }
        public int UploaderId { get; set; }
    }
}
