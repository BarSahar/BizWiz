using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizWizProj.Models
{
    public class User
    {
        int ID { get; set; }
        string userName { get; set; }
        string password { get; set; }
        string email { get; set; }
        string phoneNumber { get; set; }
        DateTime startDate { get; set; }
    }
}