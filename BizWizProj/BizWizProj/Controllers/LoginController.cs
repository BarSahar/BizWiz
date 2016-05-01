﻿using BizWizProj.Context;
using BizWizProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizWizProj.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string name, string password)
        {
            DB1 db = new DB1();

            //TODO: change password to "admin" "backdoor"
            if ("1".Equals(name) && "1".Equals(password))
            {
                Session["user"] = new BizUser() { userName = name, employeeType ="4" };
                    return RedirectToAction("Index","Home");
            }
            //for real users.
            List<BizUser> userList = db.BizUsers.ToList();
            foreach (var item in userList)
            {
                if(item.userName.Equals(name) && item.password.Equals(password))
                {
                    Session["user"] = new BizUser() {
                        userName = item.userName,
                        employeeType = item.employeeType,
                    };
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}