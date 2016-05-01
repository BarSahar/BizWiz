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
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events.Calendar;

namespace BizWizProj.Controllers
{


    public class openShiftsController : Controller
    {

        public ActionResult Backend()
        {
            return new Dpc().CallBack(this);
        }

        class Dpc : DayPilotCalendar
        {

            private DB1 dc = new DB1();

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



        private DB1 db = new DB1();
        

        // GET: openShifts
        public ActionResult Index()
        {
            return View(db.ShiftInProgress.ToList());
        }

        // GET: openShifts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            openShift openShift = db.ShiftInProgress.Find(id);
            if (openShift == null)
            {
                return HttpNotFound();
            }
            return View(openShift);
        }

        // GET: openShifts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: openShifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,dayIndex,shiftIndex,numOfEmployees,startHour,endHour,weekDate")] openShift openShift)
        {
            if (ModelState.IsValid)
            {
                db.ShiftInProgress.Add(openShift);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(openShift);
        }

        // GET: openShifts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            openShift openShift = db.ShiftInProgress.Find(id);
            if (openShift == null)
            {
                return HttpNotFound();
            }
            return View(openShift);
        }

        // POST: openShifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,dayIndex,shiftIndex,numOfEmployees,startHour,endHour,weekDate")] openShift openShift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(openShift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(openShift);
        }

        // GET: openShifts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            openShift openShift = db.ShiftInProgress.Find(id);
            if (openShift == null)
            {
                return HttpNotFound();
            }
            return View(openShift);
        }

        // POST: openShifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            openShift openShift = db.ShiftInProgress.Find(id);
            db.ShiftInProgress.Remove(openShift);
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
