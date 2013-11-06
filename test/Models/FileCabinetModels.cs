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
        [Key]
        [Required]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int FileId { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public int BandId { get; set; }
        [ForeignKey("BandId")]
        public virtual BandProfile BandProfile { get; set; }

        [Required]
        public int UploaderId { get; set; }
        [ForeignKey("UploaderId")]
        public virtual UserProfile UploaderProfile { get; set; }
    }
}
