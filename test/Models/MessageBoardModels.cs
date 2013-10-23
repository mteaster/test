using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test.Models
{
    [Table("MessageBoardPost")]
    public class MessageBoardPost
    {
        [Key]
        [Required]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        [Required]
        public int BandId { get; set; }
        [ForeignKey("BandId")]
        public virtual BandProfile BandProfile { get; set; }

        [Required]
        public int PosterId { get; set; }
        [ForeignKey("PosterId")]
        public virtual UserProfile PosterProfile { get; set; }

        [Required]
        public DateTime PostTime { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }

    public class PostMessageModel
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}