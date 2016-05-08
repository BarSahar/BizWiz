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
    public class OpenShiftsController : Controller
    {
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
        public ActionResult Create([Bind(Include = "ID,DayIndex,ShiftIndex,NumOfEmployees,Start,End,WeekDate,Text")] OpenShift OpenShift)
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

        // POST: OpenShifts/Delete/5
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
