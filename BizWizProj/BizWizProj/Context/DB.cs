using BizWizProj.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BizWizProj.Context
{
    public class DB
    {
        public DB()
        {

        }
        public DbSet<closedShift> closedShifts { get; set; }
        public DbSet<openShift> shiftInProgress { get; set; }
        public DbSet<BizUser> Users { get; set; }
        public DbSet<stockItem> stocks { get; set; }
    }
}