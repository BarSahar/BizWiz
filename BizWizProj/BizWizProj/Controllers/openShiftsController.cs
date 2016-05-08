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
using BizWizProj.Authorization;

namespace BizWizProj.Controllers
{
    [MyAuthorize]
    public class openShiftsController : Controller
    {
        private DB db = new DB();

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
        public ActionResult Create([Bind(Include = "ID,dayIndex,shiftIndex,numOfEmployees,Start,End,weekDate,Text")] openShift openShift)
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
        public ActionResult Edit([Bind(Include = "ID,dayIndex,shiftIndex,numOfEmployees,Start,End,weekDate,Text")] openShift openShift)
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
