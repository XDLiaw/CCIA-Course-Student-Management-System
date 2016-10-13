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
using System.Web.Configuration;
using System.IO;
using CCIA2.Helper.ExcelReport;
using NPOI.SS.UserModel;

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
            model.studentViewModel.studentPagedList = searchStudent(model.studentViewModel);

            ViewBag.groupList = DropDownListHelper.getAppraiseGroupList(true);
            ViewBag.courseClassList = DropDownListHelper.getCourseClassList(true);
            ViewBag.memberTypeList = DropDownListHelper.getMemberTypeList(true);
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

            courseList = query.OrderBy(c => c.startTime).ToPagedList(model.pageNumber - 1, model.pageSize);
            return courseList;
        }

        public ActionResult CreateCourse()
        {
            Course model = new Course() { day = DateTime.Now, startTime = null, endTime = null};
            ViewBag.courseClassList = DropDownListHelper.getCourseClassList(false);
            ViewBag.teacherList = DropDownListHelper.getTeacherList();
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateCourse(Course model)
        {
            if (ModelState.IsValid)
            {
                model.maxStudentNum = model.maxGroup1StudentNum + model.maxGroup2StudentNum + model.maxGroup3StudentNum;
                model.hour = (model.endTime.GetValueOrDefault() - model.startTime.GetValueOrDefault()).TotalHours;
                List<CourseTeacherRelation> tempRelation = model.teachers.ToList();
                model.teachers = null;
                db.Course.Add(model);
                db.SaveChanges();

                tempRelation.ForEach(x => x.courseSqno = model.sqno);
                db.CourseTeacherRelation.AddRange(tempRelation);
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

        public ActionResult EditCourse(int sqno)
        {
            Course model = db.Course.Where(c => c.sqno == sqno).FirstOrDefault();
            if (model == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.courseClassList = DropDownListHelper.getCourseClassList(false);
                ViewBag.teacherList = DropDownListHelper.getTeacherList();
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult EditCourse(Course model)
        {
            if (ModelState.IsValid)
            {
                model.maxStudentNum = model.maxGroup1StudentNum + model.maxGroup2StudentNum + model.maxGroup3StudentNum;
                model.hour = (model.endTime.GetValueOrDefault() - model.startTime.GetValueOrDefault()).TotalHours;
                List<CourseTeacherRelation> oldRelations = db.CourseTeacherRelation.Where(x => x.courseSqno == model.sqno).ToList();
                db.CourseTeacherRelation.RemoveRange(oldRelations);
                db.CourseTeacherRelation.AddRange(model.teachers);
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

        public ActionResult DeleteCourse(int sqno)
        {
            Course model = db.Course.Where(cc => cc.sqno == sqno).FirstOrDefault();
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
                bool isCourseHasStudent = db.MemberCourse.Where(x => x.CourseSqno == sqno).Count() > 0;
                if (isCourseHasStudent)
                {
                    var result = new { success = false, errorMessage = "此課程已有學生選修不可刪除" };
                    return Json(result);
                }
                else
                {
                    if (model.teachers != null && model.teachers.Count > 0)
                    {
                        db.CourseTeacherRelation.RemoveRange(model.teachers);
                    }
                    db.Course.Remove(model);
                    db.SaveChanges();
                    var result = new { success = true };
                    return Json(result);
                }
            }
        }

        public ActionResult CourseStudentList(int sqno)
        {
            CourseStudentListViewModel model = new CourseStudentListViewModel();
            model.course = db.Course.Where(c => c.sqno == sqno).FirstOrDefault();
            model.memberCourseList = db.MemberCourse.Where(mc => mc.CourseSqno == sqno).ToList();
            if (model.course == null)
            {
                ViewBag.ErrorMessage = "找不到課程資料";
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult EditCourseStudentAttend(int sqno, string IsAttend)
        {
            MemberCourse model = db.MemberCourse.Where(x => x.sqno == sqno).FirstOrDefault();
            if (model == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
                return RedirectToAction("Index");
            }
            else
            {
                model.IsAttend = IsAttend;
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                var result = new { success = true };
                return Json(result);
            }
        }

        public FileContentResult DownloadMemberCourseAttchFile(int sqno)
        {
            MemberCourseAttchFile attachF = db.MemberCourseAttchFile.Where(x => x.sqno == sqno).FirstOrDefault();
            if (attachF == null)
            {
                return null;
            }
            else
            {
                string folder = WebConfigurationManager.AppSettings["MemberCourseAttchFileDir"];
                string filePath = System.IO.Path.Combine(folder, attachF.mrNumber, attachF.AttchFileName);
                string contentType = FileUtils.GetContentTypeForFileName(attachF.AttchFileName);
                if (System.IO.File.Exists(filePath) == false)
                {
                    return null;
                }
                else
                {
                    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[fs.Length];
                        fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                        return new FileContentResult(bytes, contentType)
                        {
                            FileDownloadName = attachF.ShowAttchFileName
                        };
                    }
                }
            }
        }

        public ActionResult DownloadCourseStudentListReport(int sqno)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                List<MemberCourse> memberCourseList = db.MemberCourse.Where(mc => mc.CourseSqno == sqno).ToList();
                CourseStudentListReport report = new CourseStudentListReport();
                IWorkbook wb = report.create(memberCourseList);
                wb.Write(memoryStream);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "課程修課學生列表.xls");
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
                teacher.savingPhoto();
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
                teacher.savingPhoto();
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

        public ActionResult DeleteTeacher(int sqno)
        {
            CourseTeacher model = db.CourseTeacher.Where(cc => cc.sqno == sqno).FirstOrDefault();
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
                if (model.courses != null && model.courses.Count > 0)
                {
                    db.CourseTeacherRelation.RemoveRange(model.courses);
                }
                db.CourseTeacher.Remove(model);
                db.SaveChanges();
                var result = new { success = true };
                return Json(result);
            }
        }

        #endregion

        #region 學生

        private IPagedList<Member> searchStudent(StudendViewModel model)
        {
            IPagedList<Member> studentList;
            IQueryable<Member> studentQueryable = db.Member.Where(m => m.mrIsActive == "Y" && m.mrIsFinish == "Y");

            if (model.searchText != null && model.searchText.Trim().Length > 0)
            {
                studentQueryable = studentQueryable.Where(x => x.mrNumber.Contains(model.searchText) || x.mrName.Contains(model.searchText));                   
            }
            if (model.memberTypeSqno != null && model.memberTypeSqno > 0)
            {
                studentQueryable = studentQueryable.Where(x => x.mrMemberTypesqno == model.memberTypeSqno);
            }

            studentList = studentQueryable.OrderBy(t => t.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
            return studentList;
        }

        public ActionResult StudentChoseCourseList(int sqno)
        {
            StudentChoseCourseListViewModel model = new StudentChoseCourseListViewModel();
            model.student = db.Member.Where(m => m.sqno == sqno).FirstOrDefault();
            model.courseList = db.MemberCourse.Where(mc => mc.mrSqno == sqno).ToList();
            if (model.student == null)
            {
                ViewBag.ErrorMessage = "找不到學生資料";
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult StudentAttendSummary(int sqno)
        {
            StudentCourseAttendSummaryViewModel model = new StudentCourseAttendSummaryViewModel();
            model.student = db.Member.Where(m => m.sqno == sqno).FirstOrDefault();
            model.courseList = db.MemberCourse.Where(mc => mc.mrSqno == sqno).ToList();
            if (model.student == null)
            {
                ViewBag.ErrorMessage = "找不到學生資料";
                return RedirectToAction("Index");
            }
            if (model.student.memberType.membertypeno != 1)
            {
                ViewBag.ErrorMessage = "不是經紀中介學員";
                return RedirectToAction("Index");
            }

            model.nonElectiveCourseGroupList = db.CourseGroup
                .Where(g => g.tableGroup.GroupName == model.student.FinalGroup && g.isElective == false)
                .OrderBy(g => g.memberGroupSqno).ThenBy(g => g.isElective).ThenBy(g => g.courseClassSqno).ToList();
            model.electiveCourseGroupList = db.CourseGroup
                .Where(g => g.tableGroup.GroupName == model.student.FinalGroup && g.isElective == true)
                .OrderBy(g => g.memberGroupSqno).ThenBy(g => g.isElective).ThenBy(g => g.courseClassSqno).ToList();

            foreach (CourseGroup cg in model.nonElectiveCourseGroupList)
            {
                int courseClassSqno = cg.courseClass.sqno;
                double attendHour = model.courseList.Where(c => c.IsAttend == "Y" && c.course.courseClassSqno == courseClassSqno).Sum(c => c.course.hour).GetValueOrDefault(0);
                model.nonElectiveCourseAttendHourList.Add(cg, attendHour);
                if (cg.courseClass.name == "文創經紀人專業知識")
                {
                    model.nonElectiveCourseMustAttendHourList.Add(cg, 12);
                }
                if (cg.courseClass.name == "文創經紀模式")
                {
                    model.nonElectiveCourseMustAttendHourList.Add(cg, 6);
                }
                if (cg.courseClass.name == "故事行銷" || cg.courseClass.name == "圖像授權" || cg.courseClass.name == "文創科技")
                {
                    model.nonElectiveCourseMustAttendHourList.Add(cg, 30);
                }
            }
            foreach (CourseGroup cg in model.electiveCourseGroupList)
            {
                int courseClassSqno = cg.courseClass.sqno;
                double attendHour = model.courseList.Where(c => c.IsAttend == "Y" && c.course.courseClassSqno == courseClassSqno).Sum(c => c.course.hour).GetValueOrDefault(0);
                model.electiveCourseAttendHour += attendHour;
            }            

            return View(model);
        }

        public ActionResult SelectCourseManaual(int sqno)
        {
           SelectCourseManaualViewModel model = new SelectCourseManaualViewModel();
           model.CoursList = db.Course.ToList();
           model.mrSqno = sqno;
           model.AlreadySelected = db.MemberCourse.Where(x => x.mrSqno == sqno).Select(x => x.CourseSqno).ToList();

           return View(model);
        }

        [HttpPost]
        public JsonResult SubmitManaualCourse(int sqno, int[] selected)
        {
           try
           {
              Member targetMember = db.Member.Where(x => x.sqno == sqno).FirstOrDefault();
              if (targetMember == null) return Json(new { result = "member not found" });
              MemberCourse temp;
              foreach (int courseid in selected)
              {
                 temp = new MemberCourse() { CourseSqno = courseid, mrSqno = sqno, mrNumber = targetMember.mrNumber, IsAttend = "Y", CreateDate = DateTime.Now };
                 db.MemberCourse.Add(temp);
                 db.SaveChanges();
              }

              return Json(new { result = "success" });
           }
           catch
           {
              return Json(new { result = "error" });
           }           
        }

        public ActionResult ExamResult(int sqno)
        {
            CourseExamResultViewModel model = new CourseExamResultViewModel();
            model.student = db.Member.Where(m => m.sqno == sqno).FirstOrDefault();
            model.qaList = db.MemberQAnswer.Where(x => x.mrSqno == sqno && x.question.years == DateTime.Now.Year.ToString()).ToList();
            if (model.student == null)
            {
                ViewBag.ErrorMessage = "找不到學生資料";
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
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
