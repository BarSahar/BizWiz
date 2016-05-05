﻿using System;
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
        public string userName { get; set; }
        public string FullName { get; set; }
        [DataType(DataType.Password)]
        public string password { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
        public string employeeType { get; set; }
        //1-Employee
        //2-Shift manager
        //3-SuperShift manager
        //4-Manager
    }
}