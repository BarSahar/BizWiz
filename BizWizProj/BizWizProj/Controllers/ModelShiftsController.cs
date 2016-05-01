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
    public class modelShiftsController : Controller
    {
        private DB1 db = new DB1();

        // GET: modelShifts
        public ActionResult Index()
        {
            return View(db.ModelShifts.ToList());
        }

        // GET: modelShifts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelShift modelShift = db.ModelShifts.Find(id);
            if (modelShift == null)
            {
                return HttpNotFound();
            }
            return View(modelShift);
        }

        // GET: modelShifts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: modelShifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,dayIndex,shiftIndex,numOfEmployees,startHour,endHour")] modelShift modelShift)
        {
            if (ModelState.IsValid)
            {
                db.ModelShifts.Add(modelShift);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(modelShift);
        }

        // GET: modelShifts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelShift modelShift = db.ModelShifts.Find(id);
            if (modelShift == null)
            {
                return HttpNotFound();
            }
            return View(modelShift);
        }

        // POST: modelShifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,dayIndex,shiftIndex,numOfEmployees,startHour,endHour")] modelShift modelShift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modelShift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(modelShift);
        }

        // GET: modelShifts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            modelShift modelShift = db.ModelShifts.Find(id);
            if (modelShift == null)
            {
                return HttpNotFound();
            }
            return View(modelShift);
        }

        // POST: modelShifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            modelShift modelShift = db.ModelShifts.Find(id);
            db.ModelShifts.Remove(modelShift);
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
