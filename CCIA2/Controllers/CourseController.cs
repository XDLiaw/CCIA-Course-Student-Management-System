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
    public class CourseController : Controller
    {
        private CCIAContext db = new CCIAContext();

        public ActionResult Index()
        {
            CourseViewModel model = new CourseViewModel();
            return Index(model);
        }

        [HttpPost]
        public ActionResult Index(CourseViewModel model)
        {            
            model.teacherViewModel.teacherPagedList = searchTeacher(model.teacherViewModel);


            return View(model);
        }

        private IPagedList<CourseTeacher> searchTeacher(TeacherViewModel model)
        {
            IPagedList<CourseTeacher> teacherList;
            if (model.searchText != null && model.searchText.Trim().Length > 0)
            {
                teacherList = db.CourseTeacher
                    .Where(t => t.name.Contains(model.searchText) || t.orgName.Contains(model.searchText))
                    .OrderBy(t => t.name)
                    .ToPagedList(model.pageNumber - 1, model.pageSize);
            }
            else
            {
                teacherList = db.CourseTeacher.OrderBy(t => t.name).ToPagedList(model.pageNumber - 1, model.pageSize);
            }

            return teacherList;
        }

        public ActionResult CreateTeacher()
        {
            CourseTeacher teacher = new CourseTeacher();
            return View(teacher);
        }

        [HttpPost]
        public ActionResult CreateTeacher(CourseTeacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.CourseTeacher.Add(teacher);
                db.SaveChanges();

                var result = new { success = true };
                return Json(result);
            }
            else
            {
                var result = new
                {
                    success = false,
                    errorMessage = "資料有誤，請檢查並更正資料",
                    ModelStateErrors = ModelState.Where(x => x.Value.Errors.Count > 0)
                        .ToDictionary(k => k.Key, k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                };
                return Json(result);
            }
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

                var result = new { success = true };
                return Json(result);
            }
            else
            {
                var result = new
                {
                    success = false,
                    errorMessage = "資料有誤，請檢查並更正資料",
                    ModelStateErrors = ModelState.Where(x => x.Value.Errors.Count > 0)
                        .ToDictionary(k => k.Key, k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                };
                return Json(result);
            }
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
