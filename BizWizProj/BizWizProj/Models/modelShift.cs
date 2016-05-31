using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BizWizProj.Models
{
    public class ModelShift
    {
        [Key]
        public int ID { get; set; }
        public int DayIndex { get; set; }
        public int ShiftIndex { get; set; }
        [Range(1, Int32.MaxValue)]
        public int NumOfEmployees { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        //Calander plug in requires text field.
        public string Text { get; set; }
    }
}