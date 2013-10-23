using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test.Models
{
    public class MessageBoardPost
    {
        public int ID { get; set; }
        public int BandID { get; set; }
        public string UserName { get; set; }
        public DateTime PostTime { get; set; }
        public string Content { get; set; }
    }
   
}