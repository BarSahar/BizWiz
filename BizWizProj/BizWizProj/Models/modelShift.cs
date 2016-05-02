using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BizWizProj.Models
{
    public class modelShift
    {
        [Key]
        public int ID { get; set; }
        public int dayIndex { get; set; }
        public int shiftIndex { get; set; }
        public int numOfEmployees { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        //Calander plug in requires text field.
        public string Text { get; set; }
    }
}