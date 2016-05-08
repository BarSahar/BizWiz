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
    public class ClosedShiftsController : Controller
    {
        public ActionResult Backend()
        {
            return new Dpc().CallBack(this);
        }
        //TODO: maybe remove below this
        public ActionResult Events(DateTime? start, DateTime? end)
        {
            var events = from ev in db.ShiftHistory.AsEnumerable() where !(ev.End <= start || ev.Start >= end) select ev;

            var result = events
            .Select(e => new JsonEvent()
            {
                start = e.Start.ToString("s"),
                end = e.End.ToString("s"),
                text = e.Text,
                id = e.ID.ToString()
            })
            .ToList();
            return new JsonResult { Data = result };
        }

        private class JsonEvent
        {
            public string id { get; set; }
            public string text { get; set; }
            public string start { get; set; }
            public string end { get; set; }
        }
        //TODO: maybe remove above this

        class Dpc : DayPilotCalendar
        {

            private DB dc = new DB();

            protected override void OnCommand(CommandArgs e)
            {
                switch (e.Command)
                {
                  

                    case "today":
                        StartDate = DateTime.Today;
                        Update(CallBackUpdateType.Full);
                   
                        break;
                    case "navigate":
                        //Console.WriteLine(StartDate.ToString);
                       // StartDate = (DateTime)e.Data["start"];
                        Update(CallBackUpdateType.Full);
                        break;
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

                Events = from e in dc.ShiftHistory select e;
            }
        }

        private DB db = new DB();

        // GET: ClosedShifts
        public ActionResult Index()
        {
            return View(db.ShiftHistory.ToList());
        }

        // GET: ClosedShifts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClosedShift ClosedShift = db.ShiftHistory.Find(id);
            if (ClosedShift == null)
            {
                return HttpNotFound();
            }
            return View(ClosedShift);
        }

        // GET: ClosedShifts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClosedShifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DayIndex,ShiftIndex,Start,End,WeekDate")] ClosedShift ClosedShift)
        {
            if (ModelState.IsValid)
            {
                db.ShiftHistory.Add(ClosedShift);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ClosedShift);
        }

        // GET: ClosedShifts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClosedShift ClosedShift = db.ShiftHistory.Find(id);
            if (ClosedShift == null)
            {
                return HttpNotFound();
            }
            return View(ClosedShift);
        }

        // POST: ClosedShifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DayIndex,ShiftIndex,Start,End,WeekDate")] ClosedShift ClosedShift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ClosedShift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ClosedShift);
        }

        // GET: ClosedShifts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClosedShift ClosedShift = db.ShiftHistory.Find(id);
            if (ClosedShift == null)
            {
                return HttpNotFound();
            }
            return View(ClosedShift);
        }

        // POST: ClosedShifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClosedShift ClosedShift = db.ShiftHistory.Find(id);
            db.ShiftHistory.Remove(ClosedShift);
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
