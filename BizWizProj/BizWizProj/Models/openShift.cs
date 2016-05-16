using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BizWizProj.Models
{
    public class OpenShift
    {
        [Key]
        public int ID { get; set; }
        public List<BizUser> PotentialWorkers { get; set; }

        public int DayIndex { get; set; }
        public int ShiftIndex { get; set; }
        public int NumOfEmployees { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateTime WeekDate { get; set; }
        public BizUser ShiftManager { get; set; }
        public List<BizUser> Workers { get; set; }

        //Text to display Workers
        public string Text { get; set; }

    }
}