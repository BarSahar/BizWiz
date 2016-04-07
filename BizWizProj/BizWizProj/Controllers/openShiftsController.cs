using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BizWizProj.Models;

namespace BizWizProj.Controllers
{
    public class openShiftsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: openShifts
        public ActionResult Index()
        {
            return View(db.openShifts.ToList());
        }

        // GET: openShifts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            openShift openShift = db.openShifts.Find(id);
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
        public ActionResult Create([Bind(Include = "ID,numOfEmployees,dayIndex,shiftIndex,weekDate,startHour,endHour")] openShift openShift)
        {
            if (ModelState.IsValid)
            {
                db.openShifts.Add(openShift);
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
            openShift openShift = db.openShifts.Find(id);
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
        public ActionResult Edit([Bind(Include = "ID,numOfEmployees,dayIndex,shiftIndex,weekDate,startHour,endHour")] openShift openShift)
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
            openShift openShift = db.openShifts.Find(id);
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
            openShift openShift = db.openShifts.Find(id);
            db.openShifts.Remove(openShift);
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
