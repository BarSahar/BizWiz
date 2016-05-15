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
                    case "initNav":
                        StartDate = new DateTime(2016, 05, 01);
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
            return View(db.ShiftInProgress.ToList());
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
        public ActionResult Edit([Bind(Include = "ID,DayIndex,,NumOfEmployees,Start,End")] OpenShift OpenShift)
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