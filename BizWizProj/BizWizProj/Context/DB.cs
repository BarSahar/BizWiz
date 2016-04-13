using BizWizProj.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BizWizProj.Context
{
    public class DB:DbContext
    {
        public DB()
        {

        }

        public DbSet<modelShift> ModelShifts { get; set; }
        public DbSet<closedShift> ShiftHistory { get; set; }
        public DbSet<openShift> ShiftInProgress { get; set; }
        public DbSet<BizUser> BizUsers { get; set; }
        public DbSet<stockItem> Stocks { get; set; }
    }
}