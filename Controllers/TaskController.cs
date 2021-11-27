using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using OnlinePlanner.Models;
using System.Net;

namespace OnlinePlanner.Controllers
{
    public class TaskController : Controller
    {

        private OnlinePlannerEntities1 db = new OnlinePlannerEntities1();
        // GET: Task
        public ActionResult List()
        {
            // Query to entity for active record and created by logged in User
            int userID =Convert.ToInt32 (Session["UserID"]);
            var studentList = db.Tasks.SqlQuery("Select * from Task where IsDeleted=0 and CreatedBy="+userID)
                      .ToList<Task>();
            return View(studentList);
        }

        // GET: course/Create
        public ActionResult AddTask()
        {
            return View();
        }

        // POST: course/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTask( Task task)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    task.CreatedBy= Convert.ToInt32(Session["UserID"]);
                    db.Tasks.Add(task);
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(task);
        }

        public ActionResult EditTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTask(Task task)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Edit task will save date as a last updated date
                    task.LastUpdated = Convert.ToString(DateTime.Now);
                    db.Entry(task).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(task);
        }


        // GET: course/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            else
            {
                //Softdelete,by setting IsDeleted flag true 
                task.IsDeleted = true;
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
        }

       
   
        
    }
}