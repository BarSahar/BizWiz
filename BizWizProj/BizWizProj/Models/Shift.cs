using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizWizProj.Models
{
    public class Shift
    {
        int ID { get; set; }
        int dayIndex { get; set; }
        int shiftIndex { get; set; }
        DateTime weekDate { get; set; }
        TimeSpan startHour { get; set; }
        TimeSpan endHour { get; set; }
        List<Employee> workers { get; set; }

    }
}