using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BizWizProj.Models
{
    public class SystemNotices
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime SentDate { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }//1,2,3 or all employee types
    }
}