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
    public class stockItemsController : Controller
    {
        private DB db = new DB();

        // GET: stockItems
        public ActionResult Index()
        {
            return View(db.Stocks.ToList());
        }

        // GET: stockItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stockItem stockItem = db.Stocks.Find(id);
            if (stockItem == null)
            {
                return HttpNotFound();
            }
            return View(stockItem);
        }

        // GET: stockItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: stockItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,name,quantity,category,notes")] stockItem stockItem)
        {
            if (ModelState.IsValid)
            {
                db.Stocks.Add(stockItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stockItem);
        }

        // GET: stockItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stockItem stockItem = db.Stocks.Find(id);
            if (stockItem == null)
            {
                return HttpNotFound();
            }
            return View(stockItem);
        }

        // POST: stockItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,name,quantity,category,notes")] stockItem stockItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stockItem);
        }

        // GET: stockItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            stockItem stockItem = db.Stocks.Find(id);
            if (stockItem == null)
            {
                return HttpNotFound();
            }
            return View(stockItem);
        }

        // POST: stockItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            stockItem stockItem = db.Stocks.Find(id);
            db.Stocks.Remove(stockItem);
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
