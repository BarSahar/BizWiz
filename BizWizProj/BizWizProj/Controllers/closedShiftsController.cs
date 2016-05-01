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

namespace BizWizProj.Controllers
{
    public class closedShiftsController : Controller
    {
        private DB1 db = new DB1();

        // GET: closedShifts
        public ActionResult Index()
        {
            return View(db.ShiftHistory.ToList());
        }

        // GET: closedShifts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            closedShift closedShift = db.ShiftHistory.Find(id);
            if (closedShift == null)
            {
                return HttpNotFound();
            }
            return View(closedShift);
        }

        // GET: closedShifts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: closedShifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,dayIndex,shiftIndex,startHour,endHour,weekDate")] closedShift closedShift)
        {
            if (ModelState.IsValid)
            {
                db.ShiftHistory.Add(closedShift);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(closedShift);
        }

        // GET: closedShifts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            closedShift closedShift = db.ShiftHistory.Find(id);
            if (closedShift == null)
            {
                return HttpNotFound();
            }
            return View(closedShift);
        }

        // POST: closedShifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,dayIndex,shiftIndex,startHour,endHour,weekDate")] closedShift closedShift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(closedShift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(closedShift);
        }

        // GET: closedShifts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            closedShift closedShift = db.ShiftHistory.Find(id);
            if (closedShift == null)
            {
                return HttpNotFound();
            }
            return View(closedShift);
        }

        // POST: closedShifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            closedShift closedShift = db.ShiftHistory.Find(id);
            db.ShiftHistory.Remove(closedShift);
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
