﻿using System;
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
    public class SystemNoticesController : Controller
    {
        private DB db = new DB();

        // GET: SystemNotices
        public ActionResult Index()
        {
            return View(db.Notices.ToList());
        }

        // GET: SystemNotices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemNotices systemNotices = db.Notices.Find(id);
            if (systemNotices == null)
            {
                return HttpNotFound();
            }
            return View(systemNotices);
        }

        // GET: SystemNotices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemNotices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Text,SentDate,Sender,Receiver")] SystemNotices systemNotices)
        {
            if (ModelState.IsValid)
            {
                
                db.Notices.Add(systemNotices);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(systemNotices);
        }

        // GET: SystemNotices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemNotices systemNotices = db.Notices.Find(id);
            if (systemNotices == null)
            {
                return HttpNotFound();
            }
            return View(systemNotices);
        }

        // POST: SystemNotices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Text,SentDate,Sender,Receiver")] SystemNotices systemNotices)
        {
            if (ModelState.IsValid)
            {
                db.Entry(systemNotices).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(systemNotices);
        }

        // GET: SystemNotices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemNotices systemNotices = db.Notices.Find(id);
            if (systemNotices == null)
            {
                return HttpNotFound();
            }
            return View(systemNotices);
        }

        // POST: SystemNotices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SystemNotices systemNotices = db.Notices.Find(id);
            db.Notices.Remove(systemNotices);
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
