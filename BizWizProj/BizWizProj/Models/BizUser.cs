using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BizWizProj.Models
{
    public class BizUser
    {
        [Key]
        public int ID { get; set; }
        public string FullName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string EmployeeType { get; set; }

        public virtual ICollection<OpenShift> OpenShift { get; set; }
        public virtual ICollection<ClosedShift> ClosedShift { get; set; }
 
        //1-Employee
        //2-Shift manager
        //3-SuperShift manager
        //4-Manager
    }
}