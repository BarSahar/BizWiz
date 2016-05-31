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
        public static HttpSessionStateBase CurrentSession;

        public ActionResult Backend()
        {
            CurrentSession = this.Session;
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

            protected override void OnBeforeEventRender(BeforeEventRenderArgs e)
            {
                if (CurrentSession["user"] == null || !dc.ShiftInProgress.Any())
                    return;
                int currentUserId = ((BizUser)CurrentSession["user"]).ID;

                OpenShift tempShift = dc.ShiftInProgress.Find(int.Parse(e.Id));
                if (tempShift.PotentialWorkers.Count >= 0)
                {
                    foreach (UserPref worker in tempShift.PotentialWorkers.ToList())
                    {
                        if (worker.UserID == currentUserId)
                        {
                            switch (worker.Preference)
                            {
                                case 1:
                                    e.BackgroundColor = "green";
                                    break;
                                case 2:
                                    e.BackgroundColor = "blue";
                                    break;
                                case 3:
                                    e.BackgroundColor = "red";
                                    break;
                            }
                            break;
                        }
                    }
                }
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

        public ActionResult EditShift(string ShiftID) //Bar - for the supershift manager to edit the schedule
        {
            OpenShift tempshift = db.ShiftInProgress.Find(int.Parse(ShiftID));
            if (tempshift != null)
            {
                //Gathering remaining employees that didn't send shifts
                var potentialEmps = db.BizUsers.ToList()
                    .Where(u => tempshift.PotentialWorkers.All(w => w.UserID != u.ID) && u.EmployeeType == "1")
                    .Select(u => new UserPref() { UserID = u.ID, Preference = 0, UserName = u.FullName, IsManager = false });
                //Gathering remaining shift managers that didn't send shifts
                var potentialManagers = db.BizUsers.ToList()
                    .Where(u => tempshift.PotentialWorkers.All(w => w.UserID != u.ID) && (u.EmployeeType == "2" || u.EmployeeType == "3"))
                    .Select(u => new UserPref() { UserID = u.ID, Preference = 0, UserName = u.FullName, IsManager = true });

                var AllOtherUsers = potentialManagers.Concat(potentialEmps).ToList();
                
                //Adding those users to potential workers for this shift
                foreach (UserPref user in AllOtherUsers)
                {
                    tempshift.PotentialWorkers.Add(user);
                }

                tempshift.PotentialWorkers = tempshift.PotentialWorkers.OrderBy(u => u.IsManager).Reverse().ToList();
                return View(tempshift);
            }
            return View();
        }

        [HttpPost]
        public ActionResult SendShift(int shiftID, int preference) //Bar - employee registers to a shift
        {
            if (!db.ShiftInProgress.Any() || !db.BizUsers.Any() || Session["user"] == null)  // checking if open shift and user list is empty and user in Session
                return RedirectToAction("Index");

            int senderID = ((BizUser)Session["user"]).ID;
            OpenShift tempShift = db.ShiftInProgress.Find(shiftID);
            if (tempShift != null)
            {
                // a flag to check if there should be another
                bool AddnewPref = true;
                foreach (UserPref worker in tempShift.PotentialWorkers.ToList())
                {
                    if (worker.UserID == senderID)
                    {
                        worker.Preference = preference;
                        AddnewPref = false;
                    }
                }
                if (AddnewPref == true)
                {
                    bool isManager = false;
                    if (db.BizUsers.Find(senderID).EmployeeType == "2" || db.BizUsers.Find(senderID).EmployeeType == "3")
                        isManager = true;
                    tempShift.PotentialWorkers.Add(new UserPref() { UserID = senderID, Preference = preference, UserName = db.BizUsers.Find(senderID).FullName, IsManager = isManager });
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public ActionResult SaveShift_Manager(FormCollection formCollection, string shiftId) //Bar - SuperShiftManager assignes a shiftmanager to a shift
        {
            OpenShift currentShift = db.ShiftInProgress.Find(int.Parse(shiftId));
            foreach(string key in formCollection.AllKeys)
            {
                if (formCollection[key]!=null)
                {
                    BizUser temp = db.BizUsers.Find(int.Parse(formCollection[key]));
                    currentShift.ShiftManager = temp;
                    currentShift.UpdateText();
                    db.SaveChanges();
                    break;
                }
            }
            return View("SucOpenShift");
        }

        [HttpPost]
        public ActionResult SaveShift_Employees(FormCollection formCollection, string shiftId) //Bar - SuperShiftManager assignes workers to a shift
        {
            OpenShift currentShift = db.ShiftInProgress.Find(int.Parse(shiftId));
            List<BizUser> newlist = new List<BizUser>();
            foreach (string key in formCollection.AllKeys)
            {
                if (formCollection[key].Contains("true"))
                {
                    BizUser temp = db.BizUsers.Find(int.Parse(key));
                    if (temp!=null)
                        newlist.Add(temp);
                }
            }
            currentShift.Workers = newlist;
            currentShift.UpdateText();
            db.SaveChanges();
            return View("SucOpenShift");
        }

        [HttpPost]
        public ActionResult OpenToClose() //Avi OpenShift-->CloseShift
        {
            List<OpenShift> openShiftList = new List<OpenShift>();
            if (!db.ShiftInProgress.Any())  // checking if open shift is empty
                return RedirectToAction("Index");
            openShiftList = db.ShiftInProgress.ToList(); //loading all date from open shift table
            List<ClosedShift> newCloseShiftsList = new List<ClosedShift>(); //creating new close shift list that gonna be added to close shift table 
            for (int i = 0; i < openShiftList.Count; i++)
            {
                newCloseShiftsList.Add(new ClosedShift()
                {
                    DayIndex = openShiftList[i].DayIndex,
                    ShiftIndex = openShiftList[i].ShiftIndex,
                    Start = openShiftList[i].Start,
                    End = openShiftList[i].End,
                    WeekDate = openShiftList[i].WeekDate,
                    ShiftManager = openShiftList[i].ShiftManager,
                    Workers = openShiftList[i].Workers,
                    Text = openShiftList[i].Text
                });
                //Remove Potential workers before deleting the table to prevent runtime exceptions
                openShiftList[i].PotentialWorkers.Clear();
            }
            db.ShiftHistory.AddRange(newCloseShiftsList); //Adding all new close shift to close shift db
            db.ShiftInProgress.RemoveRange(openShiftList);// clearing open shift db
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void ModelTopen() //Avi ModelShift--->OpenShift
        {
            DateTime shiftDate = DateTime.Now.AddDays(7); //seting date for next week
            if (db.ShiftInProgress.Any())                 // preventing override of existing data in calendar
                return;
            List<ModelShift> modelist = new List<ModelShift>();
            modelist = db.ModelShifts.ToList();               //loading all date from model-shift table 
            DateTime firstDayOfWeek = shiftDate.AddDays(-(int)shiftDate.DayOfWeek); // seting first day of next week 
            List<OpenShift> newlist = new List<OpenShift>();
            if (modelist.Any())
            {
                for (int i = 0; i < modelist.Count; i++)
                {
                    DateTime ShiftDate = firstDayOfWeek.AddDays(modelist[i].Start.Day);

                    DateTime tempStart = new DateTime(ShiftDate.Year, ShiftDate.Month, ShiftDate.Day, modelist[i].Start.Hour, modelist[i].Start.Minute, modelist[i].Start.Second);
                    DateTime tempEnd = new DateTime(ShiftDate.Year, ShiftDate.Month, ShiftDate.Day, modelist[i].End.Hour, modelist[i].Start.Minute, modelist[i].Start.Second);
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