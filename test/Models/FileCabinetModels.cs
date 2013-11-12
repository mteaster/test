using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using test.Models.Account;
using test.Models.Band;
using System;
using System.Web;

namespace test.Models.FileCabinet
{
    public enum FileType
    {
        File
    }

    [Table("FileEntry")]
    public class FileEntry
    {
        public FileEntry() {}
        public FileEntry(string fileName, int bandId, int groupId, int uploaderId, FileType fileType, int fileSize, string fileDescription, DateTime modifiedTime)
        {
            this.FileName = fileName;
            this.GroupId = groupId;
            this.BandId = bandId;
            this.UploaderId = uploaderId;
            this.FileType = (int)fileType;
            this.FileSize = fileSize;
            this.FileDescription = fileDescription;
            this.ModifiedTime = modifiedTime;
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

        [Required]
        public int FileType { get; set; }

        [Required]
        public int FileSize { get; set; }

        [Required]
        public string FileDescription { get; set; }

        [Required]
        public DateTime ModifiedTime { get; set; }
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
        public FileEntryModel(int fileId, string fileName, string fileDescription, FileType fileType, int fileSize, string uploaderName, DateTime modifiedTime)
        {
            this.FileId = fileId;
            this.FileName = fileName;
            this.FileDescription = fileDescription;
            this.FileType = fileType.ToString();
            this.UploaderName = uploaderName;
            this.ModifiedTime = modifiedTime;
        }

        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public string UploaderName { get; set; }
        public DateTime ModifiedTime { get; set; }
    }

    public class UploadFileModel
    {
        [Required]
        [Display(Name = "Filename")]
        public HttpPostedFileBase File { get; set; }

        [Required]
        [Display(Name = "File description")]
        public string FileDescription { get; set; }
    }
}
