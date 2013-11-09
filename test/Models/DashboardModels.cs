using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using test.Models.Band;
using test.Models.Account;

namespace test.Models.Dashboard
{
    public enum PostType
    {
        Message,
        MemberJoin,
        MemberLeave,
        FileUpload
    }

    [Table("MessageBoardPost")]
    public class MessageBoardPost
    {
        public MessageBoardPost() {}
        public MessageBoardPost(int bandId, int posterId, PostType postType, DateTime postTime, string content)
        {
            this.BandId = bandId;
            this.PosterId = posterId;
            this.PostType = (int)postType;
            this.PostTime = postTime;
            this.Content = content;
        }

        [Key]
        [Required]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        [Required]
        public int PostType { get; set; }

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
        public MessageBoardPostModel() {}
        public MessageBoardPostModel(int postId, int posterId, string posterName, PostType postType, DateTime postTime, string content)
        {
            this.PostId = postId;
            this.PosterId = posterId;
            this.PosterName = posterName;
            this.PostType = postType;
            this.PostTime = postTime;
            this.Content = content;
        }

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
        public PostType PostType { get; set; }

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