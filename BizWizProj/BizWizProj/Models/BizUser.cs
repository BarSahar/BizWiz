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
        [Required(ErrorMessage = "This field can not be empty.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "This field can not be empty.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "This field can not be empty.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This field can not be empty.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "This field can not be empty.")]
        public string EmployeeType { get; set; }

        public virtual ICollection<OpenShift> OpenShift { get; set; }
        public virtual ICollection<ClosedShift> ClosedShift { get; set; }
 
        //1-Employee
        //2-Shift manager
        //3-SuperShift manager
        //4-Manager
    }
}