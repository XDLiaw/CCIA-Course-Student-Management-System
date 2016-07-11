using CCIA2.Models;
using CCIA2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using System.Data.Entity;
using CCIA2.Helper;

namespace CCIA2.Controllers
{
    [Authorize]
    [SessionExpire]
    public class CourseController : Controller
    {
        private CCIAContext db = new CCIAContext();

        public ActionResult Index()
        {
            CourseRelativeViewModel model = new CourseRelativeViewModel();
            return Index(model);
        }

        [HttpPost]
        public ActionResult Index(CourseRelativeViewModel model)
        {

            model.courseGroupViewModel.courseGroupList = searchCourseGroup(model.courseGroupViewModel);
            model.courseViewModel.CoursePagedList = searchCourse(model.courseViewModel);
            model.teacherViewModel.teacherPagedList = searchTeacher(model.teacherViewModel);


            ViewBag.groupList = DropDownListHelper.getAppraiseGroupList(true);
            ViewBag.courseClassList = DropDownListHelper.getCourseClassList(true);
            return View(model);
        }

        #region 各組須修課程

        private List<CourseGroup> searchCourseGroup(CourseGroupViewModel model)
        {
            List<CourseGroup> courseGroupList = db.CourseGroup
                .Where(g => model.groupSqno == 0 ? true : g.memberGroupSqno == model.groupSqno)
                .OrderBy(g => g.memberGroupSqno).ThenBy(g => g.isElective).ThenBy(g => g.courseClassSqno).ToList();
            return courseGroupList;
        }

        public ActionResult CreateCourseGroup()
        {
            CourseGroup courseGroup = new CourseGroup();

            ViewBag.groupList = DropDownListHelper.getAppraiseGroupList(false);
            ViewBag.courseClassList = DropDownListHelper.getCourseClassList(false);
            return View(courseGroup);
        }

        [HttpPost]
        public ActionResult CreateCourseGroup(CourseGroup model)
        {
            if (ModelState.IsValid)
            {
                db.CourseGroup.Add(model);
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

        public ActionResult EditCourseGroup(int sqno)
        {
            CourseGroup model = db.CourseGroup.Where(cc => cc.sqno == sqno).FirstOrDefault();
            if (model == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.groupList = DropDownListHelper.getAppraiseGroupList(false);
                ViewBag.courseClassList = DropDownListHelper.getCourseClassList(false);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult EditCourseGroup(CourseGroup model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
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

        public ActionResult DeleteCourseGroup(int sqno)
        {
            CourseGroup model = db.CourseGroup.Where(cc => cc.sqno == sqno).FirstOrDefault();
            if (model == null)
            {
                var result = new
                {
                    success = false,
                    errorMessage = "找不到資料",
                };
                return Json(result);
            }
            else
            {
                db.CourseGroup.Remove(model);
                db.SaveChanges();
                var result = new { success = true };
                return Json(result);
            }
        }

        #endregion

        #region 分項課程列表

        private IPagedList<Course> searchCourse(CourseViewModel model)
        {
            IPagedList<Course> courseList;
            IQueryable<Course> query = db.Course;
            if (model.courseClassSqno > 0)
            {
                query = query.Where(c => c.courseClassSqno == model.courseClassSqno);
            }
            if (model.day != null)
            {
                query = query.Where(c => c.day == model.day);
            }
            if (model.searchText != null && model.searchText.Trim().Length > 0)
            {
                query = query.Where(c => c.topic.Contains(model.searchText) || c.title.Contains(model.searchText));
            }

            courseList = query.OrderBy(c => c.courseClassSqno).ThenBy(c => c.startTime)
                .ToPagedList(model.pageNumber - 1, model.pageSize);
            return courseList;
        }

        #endregion

        #region 講師

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

        #endregion

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
