using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BizWizProj.Models
{
    public class ClosedShift
    {
        [Key]
        public int ID { get; set; }
        public int DayIndex { get; set; }
        public int ShiftIndex { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateTime WeekDate { get; set; }
        public virtual BizUser ShiftManager { get; set; }
        public virtual ICollection<BizUser> Workers { get; set; }


        //Text to display WeekDate
        public string Text { get; set; }
    }
}