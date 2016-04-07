using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizWizProj.Models
{
    public class openShift:closedShift
    {
        public int numOfEmployees { get; set; }
        public List<BizUser> potentialWorkers { get; set; }
    }
}