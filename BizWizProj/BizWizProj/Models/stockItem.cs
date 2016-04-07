using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BizWizProj.Models
{
    public class stockItem
    {
        [Key]
        public int ID { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public string category { get; set; }
        public string notes { get; set; }
    }
}