using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BizWizProj.Context;
using BizWizProj.Models;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Events.Calendar;
using DayPilot.Web.Mvc.Enums;
using BizWizProj.Authorization;

namespace BizWizProj.Controllers
{
    [MyAuthorize]
    public class OpenShiftsController : Controller
    {
        public ActionResult Backend()
        {
            return new Dpc().CallBack(this);
        }

        class Dpc : DayPilotCalendar
        {

            private DB dc = new DB();
            protected override void OnEventClick(EventClickArgs e)
            {

                base.OnEventClick(e);
            }
            protected override void OnCommand(CommandArgs e)
            {
                switch (e.Command)
                {
                    case "refresh":
                        Update();
                        break;
                    case "nextWeek":
                        StartDate = DateTime.Now.AddDays(7);
                        Update(CallBackUpdateType.Full);
                        break;
                }
            }

            protected override void OnEventDelete(EventDeleteArgs e)
            {
                int Id = Convert.ToInt32(e.Id);
                var item = (from ev in dc.ShiftInProgress where ev.ID == Id select ev).First();

                dc.ShiftInProgress.Remove(item);
                dc.SaveChanges();
                Update();
            }

            protected override void OnInit(InitArgs e)
            {
                UpdateWithMessage("Welcome!", CallBackUpdateType.Full);
            }

            protected override void OnFinish()
            {

                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }

                DataIdField = "Id";
                DataStartField = "Start";
                DataEndField = "End";
                DataTextField = "Text";

                Events = from e in dc.ShiftInProgress select e;
            }
        }


        private DB db = new DB();

        // GET: OpenShifts
        public ActionResult Index()
        {
            ModelTopen();
            return View(db.ShiftInProgress.ToList());
        }

        [HttpPost]
        public ActionResult SendShift(int shiftID, int senderID, int preference) //Bar - employee registers to a shift
        {
            if (!db.ShiftInProgress.Any()&& !db.BizUsers.Any())  // checking if open shift and user list is empty
                return RedirectToAction("Index");
            OpenShift tempShift = db.ShiftInProgress.Find(shiftID);
            BizUser tempUser = db.BizUsers.Find(senderID);
            if (tempShift!=null && tempUser!=null)
            {
                tempShift.PotentialWorkers.Add(new UserPref() { User = tempUser, Preference = preference });
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult OpenToClose() //Avi OpenShift-->CloseShift
        {
            List<OpenShift> openShiftList = new List<OpenShift>();
            if (!db.ShiftInProgress.Any())  // checking if open shift is empty
                return RedirectToAction("Index");
            openShiftList = db.ShiftInProgress.ToList(); //loading all date from open shift table
            List<ClosedShift> newCloseShiftsList = new List<ClosedShift>(); //creating new close shift list that gonna be added to close shift table 
            for(int i=0;i<openShiftList.Count;i++)
            {
                newCloseShiftsList.Add(new ClosedShift(){
                DayIndex=openShiftList[i].DayIndex,
                ShiftIndex=openShiftList[i].ShiftIndex,
                Start=openShiftList[i].Start,
                End=openShiftList[i].End,
                WeekDate=openShiftList[i].WeekDate,
                ShiftManager=openShiftList[i].ShiftManager,
                Workers=openShiftList[i].Workers,
                Text=openShiftList[i].Text
                }); 
            }
            db.ShiftHistory.AddRange(newCloseShiftsList); //Adding all new close shift to close shift db
            db.ShiftInProgress.RemoveRange(openShiftList);// clearing open shift db
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void ModelTopen() //Avi ModelShift--->OpenShift
        {
            DateTime shiftDate = DateTime.Now.AddDays(7); //seting date for next week
            if (!db.ShiftInProgress.Any())                 // preventing override of existing data in calendar
                return;
            List<ModelShift> modelist = new List<ModelShift>();
            modelist = db.ModelShifts.ToList();               //loading all date from model-shift table 
            DateTime firstDayOfWeek = shiftDate.AddDays(-(int)shiftDate.DayOfWeek); // seting first day of next week 
            List<OpenShift> newlist = new List<OpenShift>();
            if (modelist.Any())
            {
                for (int i = 0; i < modelist.Count; i++)
                {
                    int x = firstDayOfWeek.Day + (int)modelist[i].Start.DayOfWeek;
                    DateTime tempStart = new DateTime(firstDayOfWeek.Year, firstDayOfWeek.Month, x, modelist[i].Start.Hour, modelist[i].Start.Minute, modelist[i].Start.Second);
                    DateTime tempEnd = new DateTime(firstDayOfWeek.Year, firstDayOfWeek.Month, x, modelist[i].End.Hour, modelist[i].Start.Minute, modelist[i].Start.Second);
                    newlist.Add(new OpenShift()
                    {
                        DayIndex = (int)tempStart.DayOfWeek,
                        Start = tempStart,
                        End = tempEnd,
                        WeekDate = firstDayOfWeek
                    });
                }
                db.ShiftInProgress.AddRange(newlist);
                db.SaveChanges();
            }
        }
        // GET: OpenShifts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenShift OpenShift = db.ShiftInProgress.Find(id);
            if (OpenShift == null)
            {
                return HttpNotFound();
            }
            return View(OpenShift);
        }

        // GET: OpenShifts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OpenShifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DayIndex,ShiftIndex,NumOfEmployees,Start,End")] OpenShift OpenShift)
        {
            if (ModelState.IsValid)
            {
                db.ShiftInProgress.Add(OpenShift);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(OpenShift);
        }

        // GET: OpenShifts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenShift OpenShift = db.ShiftInProgress.Find(id);
            if (OpenShift == null)
            {
                return HttpNotFound();
            }
            return View(OpenShift);
        }

        // POST: OpenShifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DayIndex,ShiftIndex,NumOfEmployees,Start,End,WeekDate,Text")] OpenShift OpenShift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(OpenShift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(OpenShift);
        }

        // GET: OpenShifts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OpenShift OpenShift = db.ShiftInProgress.Find(id);
            if (OpenShift == null)
            {
                return HttpNotFound();
            }
            return View(OpenShift);
        }

        // POST: ShiftInProgress/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OpenShift OpenShift = db.ShiftInProgress.Find(id);
            db.ShiftInProgress.Remove(OpenShift);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}