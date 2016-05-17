﻿using System;
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
        public string Subject { get; set; } //title of the notice
        public string Text { get; set; }   //body message
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }//1,2,3 or all employee types

        public SystemNotices()
        {
            this.Date = DateTime.Now;
            this.From = (HttpContext.Current.Session["user"] as BizUser).FullName;
        }
    }
}