using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizWizProj.Controllers
{
    public class SystemNoticesController : Controller
    {
        // GET: SystemNotices
        public ActionResult Index()
        {
            return View();
        }

        // GET: SystemNotices/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SystemNotices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemNotices/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SystemNotices/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SystemNotices/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: SystemNotices/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SystemNotices/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
