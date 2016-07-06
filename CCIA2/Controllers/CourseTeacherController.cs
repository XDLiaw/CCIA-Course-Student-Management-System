using CCIA2.Models;
using CCIA2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using System.Data.Entity;

namespace CCIA2.Controllers
{
    [Authorize]
    [SessionExpire]
    public class CourseTeacherController : Controller
    {
        private CCIAContext db = new CCIAContext();

        public ActionResult Index()
        {
            CourseTeacherViewModel model = new CourseTeacherViewModel();
            return Index(model);
        }
        
        [HttpPost]
        public ActionResult Index(CourseTeacherViewModel model)
        {
            if (model.searchText != null && model.searchText.Trim().Length > 0)
            {
                model.teacherPagedList = db.CourseTeacher
                    .Where(t => t.name.Contains(model.searchText) || t.orgName.Contains(model.searchText))                    
                    .OrderBy(t => t.name)
                    .ToPagedList(model.pageNumber - 1, model.pageSize);
            }
            else
            {
                model.teacherPagedList = db.CourseTeacher.OrderBy(t => t.name).ToPagedList(model.pageNumber - 1, model.pageSize);
            }

            return View(model);
        }

        public ActionResult EditTeacher(int sqno)
        {
            CourseTeacher teacher = db.CourseTeacher.Where(t => t.sqno == sqno).FirstOrDefault();
            if (teacher == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
                return RedirectToAction("Index");
            }
            else
            {
                return View(teacher);
            }
        }

        [HttpPost]
        public ActionResult EditTeacher(CourseTeacher teacher)
        {
            if (ModelState.IsValid)
            {
                teacher.courses = db.CourseTeacher.Where(t => t.sqno == teacher.sqno).SelectMany(t => t.courses).ToList();
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return View("Close");
            }
            return View(teacher);
        }
    }
}
