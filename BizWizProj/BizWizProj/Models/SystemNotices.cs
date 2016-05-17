using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BizWizProj.Models;
using System.Web.Mvc;
using System.Web.Routing;
namespace BizWizProj.Models
{
    public class SystemNotices
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }   //body message
        public DateTime SentDate { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }//1,2,3 or all employee types

        private void set(HttpContextBase httpContext)
        {
            this.Sender = httpContext.Session["user"].ToString();
        }
    }
}