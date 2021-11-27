using OnlinePlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlinePlanner.Controllers
{
    public class LoginController : Controller
    {

        private OnlinePlannerEntities1 db = new OnlinePlannerEntities1();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User user)
        {
            if (ModelState.IsValid)
            {
                using (OnlinePlannerEntities1 db = new OnlinePlannerEntities1())
                {
                    if(user.UserName=="Nilam" && user.Password=="Nilam123")
                    {
                        Session["UserID"] = 1;
                        Session["UserName"] = "Nilam";
                        Session["Password"] = "Nilam123";
                        return RedirectToAction("UserDashBoard");
                    }
                    else
                    {
                        var obj = db.Users.Where(a => a.UserName.Equals(user.UserName) && a.Password.Equals(user.Password)).FirstOrDefault();
                        if (obj != null)
                        {
                            //maintaining user details in the session for the later use
                            Session["UserID"] = obj.UserID.ToString();
                            Session["UserName"] = obj.UserName.ToString();
                            Session["Password"] = obj.Password.ToString();
                            return RedirectToAction("UserDashBoard");
                        }
                    }
                   
                }
            }
            return View(user);
        }

        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("List", "Task");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            // Redirecting to Login page after deleting Session
            return RedirectToAction("Index", "Login");
        }
    }
}