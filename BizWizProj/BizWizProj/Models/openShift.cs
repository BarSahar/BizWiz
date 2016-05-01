using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BizWizProj.Models
{
    public class openShift
    {
        [Key]
        public int ID { get; set; }
        public List<BizUser> potentialWorkers { get; set; }

        public int dayIndex { get; set; }
        public int shiftIndex { get; set; }
        public int numOfEmployees { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public DateTime weekDate { get; set; }
        public BizUser shiftManager { get; set; }
        public List<BizUser> workers { get; set; }

        //Text to display workers
        public string Text { get; set; }

    }
}