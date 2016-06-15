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
        public int NumOfEmployees { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public virtual BizUser ShiftManager { get; set; }
        public virtual ICollection<BizUser> Workers { get; set; }
        public virtual ICollection<UserPref> PotentialWorkers { get; set; }
        //Text to display Workers
        public string Text { get; set; }


        public void UpdateText()
        {
            string result = "";
            if (ShiftManager!=null)
                result = result + "Manager:" + ShiftManager.FullName + " \n";
            if (Workers!=null)
            {
                if (Workers.Count>0)
                    result = result + "Employees: \n";
                foreach(BizUser user in Workers)
                {
                    result = result + user.FullName + " \n";
                }
            }
             Text = result;
        }
    }
}

