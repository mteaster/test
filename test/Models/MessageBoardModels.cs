using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace test.Models
{
    public class MessageBoardModels
    {
        public int ID { get; set; }
        public int BandID { get; set; }
        public string UserName { get; set; }
        public DateTime PostTime { get; set; }
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
    }
   
}