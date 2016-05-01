using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BizWizProj.Models
{
    public class closedShift
    {
        [Key]
        public int ID { get; set; }
        public int dayIndex { get; set; }
        public int shiftIndex { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateTime weekDate { get; set; }
        public BizUser shiftManager { get; set; }
        public List<BizUser> workers { get; set; }
    }
}