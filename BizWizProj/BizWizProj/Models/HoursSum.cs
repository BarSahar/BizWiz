
using BizWizProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizWizProj.Context;

using System.ComponentModel.DataAnnotations;


namespace BizWizProj.Models
{
    public class HoursSum
    {
        static public int IniHours(DB db, string session_name)
        {

            var counter = 0;

            BizUser e1 = new BizUser() { ID = 1, Password = "1", FullName = "1", Email = "yoni@gmail.com", PhoneNumber = "054-2290705", EmployeeType = "4" };
            BizUser e2 = new BizUser() { ID = 2, Password = "2", FullName = "bar", Email = "bar@gmail.com", PhoneNumber = "054-2290706", EmployeeType = "1" };
            BizUser e3 = new BizUser() { ID = 3, Password = "3", FullName = "avi", Email = "avi@gmail.com", PhoneNumber = "054-2290707", EmployeeType = "1" };
            BizUser e4 = new BizUser() { ID = 4, Password = "4", FullName = "elisha", Email = "elisha@gmail.com", PhoneNumber = "054-2290708", EmployeeType = "1" };
            BizUser e5 = new BizUser() { ID = 5, Password = "5", FullName = "darsher", Email = "darsher@gmail.com", PhoneNumber = "054-2290709", EmployeeType = "1" };

            List<BizUser> l1 = new List<BizUser>();
            List<BizUser> l2 = new List<BizUser>();
            l1.Add(e1);
            l1.Add(e2);
            l1.Add(e3);
            l1.Add(e4);
            l1.Add(e5);

            l2.Add(e1);
            l2.Add(e2);
            l2.Add(e3);
            l2.Add(e4);
            l2.Add(e5);


            db.SaveChanges();
            db.BizUsers.AddRange(l1);
            db.BizUsers.AddRange(l2);

            List<BizUser> userList = db.BizUsers.ToList();

            ClosedShift s1 = new ClosedShift() { WeekDate = DateTime.Now, Start = DateTime.Now, End = DateTime.Now.AddHours(3) };
            ClosedShift s2 = new ClosedShift() { WeekDate = DateTime.Now, Start = DateTime.Now, End = DateTime.Now.AddHours(3) };

            s1.Workers = new List<BizUser>();
            s2.Workers = new List<BizUser>();

            s1.Workers.AddRange(l1);
            s2.Workers.AddRange(l2);

            List<ClosedShift> cl = new List<ClosedShift>();
            cl.Add(s1);
            cl.Add(s2);

            db.ShiftHistory.Add(s1);
            db.SaveChanges();
            List<ClosedShift> shiftList = db.ShiftHistory.Local.ToList();
            shiftList.Reverse(); //start from the end
            if (shiftList != null)
            {
                foreach (var s_item in shiftList)
                {
                    if (s_item.WeekDate.Month == DateTime.Now.Month && s_item.WeekDate.Date.Year == DateTime.Now.Year)
                    {
                        if (DateTime.Now.Day <= s_item.WeekDate.Day) // wouldnt count the future days
                        {
                            List<BizUser> names = s_item.Workers;
                            if (names != null)
                            {
                                foreach (var u_item in names)
                                {
                                    //TODO : check if it works
                                    if (u_item.FullName == session_name)
                                        if ((s_item.End.Hour - s_item.Start.Hour) < 0)
                                            counter += s_item.End.Hour - s_item.Start.Hour + 24;
                                        else
                                            counter += s_item.End.Hour - s_item.Start.Hour;
                                }
                            }
                        }


                    }
                    else
                        break;
                }
            }


            //Yoni
            return counter;

        }




    }


}

