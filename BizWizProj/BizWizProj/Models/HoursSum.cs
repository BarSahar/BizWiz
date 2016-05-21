﻿
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

            


            List<ClosedShift> shiftList = db.ShiftHistory.ToList();
            shiftList.Reverse(); //start from the end
            if (shiftList != null)
            {
                foreach (var s_item in shiftList)
                {
                    if (s_item.WeekDate.Month == DateTime.Now.Month && s_item.WeekDate.Date.Year == DateTime.Now.Year)
                    {
                        if (DateTime.Now.Day <= s_item.WeekDate.Day) // wouldnt count the future days
                        {
                            ICollection<BizUser> names = s_item.Workers;
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
            return counter;
        }

        static public String AllWorkers(DB db)
        {
            var counter = 0;
            String WorkesrHours="";
            List<BizUser> WorkersList = db.BizUsers.ToList();

            if (WorkersList != null)
                foreach (var s_item in WorkersList)
                {
                    counter = IniHours(db, s_item.FullName);
                    WorkesrHours += s_item.FullName + " " + counter.ToString() + " MH. ";
                }
            else
                return "No workers Listed";
            
            return WorkesrHours;
            
        }


    }
}

