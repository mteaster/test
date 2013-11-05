using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using test.Models.Band;
using test.Models.Account;

namespace test.Models.Dashboard
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

        public int PosterId { get; set; }
        [ForeignKey("PosterId")]
        public virtual UserProfile PosterProfile { get; set; }

        [Required]
        public DateTime PostTime { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }

    public class MessageBoardPostModel
    {
        [Required]
        [Display(Name = "ID")]
        public int PostId { get; set; }

        [Required]
        [Display(Name = "Poster ID")]
        public int PosterId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string PosterName { get; set; }

        [Required]
        [Display(Name = "Time")]
        public DateTime PostTime { get; set; }

        [Required]
        [Display(Name = "Content")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }

    public class DashboardViewModel
    {
        // model for the post given to the server
        public PostMessageModel PostMessageModel { get; set; }
        // model for the messages returned by the server
        public List<MessageBoardPostModel> DisplayMessagesModel { get; set; }
    }

    public class PostMessageModel
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
}