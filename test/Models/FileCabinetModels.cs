using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using test.Models.Account;
using test.Models.Band;

namespace test.Models.FileCabinet
{
    public enum FileType
    {
        File,
        Text,
        Document,
        Image,
        Audio,
        Video
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

        public string FileDescription { get; set; }

        [Required]
        public DateTime ModifiedTime { get; set; }
    }

    [Table("FileGroup")]
    public class FileGroup
    {
        public FileGroup() { }
        public FileGroup(int bandId, string groupName)
        {
            this.BandId = bandId;
            this.GroupName = groupName;
        }

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

    // can i get rid of this guy?
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
            this.ModifiedTime = modifiedTime.ToString();
        }

        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public string FileType { get; set; }
        public int FileSize { get; set; }
        public string UploaderName { get; set; }
        public string ModifiedTime { get; set; }
    }

    public class FileGroupModel
    {
        public FileGroupModel() { }
        public FileGroupModel(int groupId, int bandId, string groupName)
        {
            this.GroupId = groupId;
            this.BandId = bandId;
            this.GroupName = groupName;
        }

        public int GroupId { get; set; }
        public int BandId { get; set; }
        public string GroupName { get; set; }
    }

    public class UploadFileModel
    {
        [Required]
        [Display(Name = "Filename")]
        public HttpPostedFileBase File { get; set; }

        [Display(Name = "File description")]
        public string FileDescription { get; set; }
    }

    public class FileModel
    {
        public FileModel() { }
        public FileModel(FileEntry entry, string uploader)
        {
            this.FileId = entry.FileId;
            this.FileName = entry.FileName;
            this.FileDescription = entry.FileDescription;
            this.FileType = entry.FileType.ToString();
            this.FileSize = entry.FileSize;
            this.GroupId = entry.GroupId;
            this.ModifiedTime = entry.ModifiedTime;
            this.UploaderName = uploader;
        }

        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public string FileType { get; set; }
        public int GroupId { get; set; }
        public int FileSize { get; set; }
        public string UploaderName { get; set; }
        public DateTime ModifiedTime { get; set; }

        public string Content { get; set; }
    }
}
