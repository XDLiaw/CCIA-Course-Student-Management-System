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
using CCIA2.Helper.ExcelReport;
using System.IO;
using NPOI.SS.UserModel;


namespace CCIA2.Controllers
{
    [Authorize]
    [SessionExpire]
    public class ActivityController : Controller
    {
        private CCIAContext db = new CCIAContext();

        //
        // GET: /Activity/
        public ActionResult Index()
        {
            ActivityViewModel model = new ActivityViewModel();
            return Index(model);
        }

        [HttpPost]
        public ActionResult Index(ActivityViewModel model)
        {
            model.activityPagedList = db.activity.OrderByDescending(x => x.startDate).ToPagedList(model.pageNumber - 1, model.pageSize);
            return View(model);
        }

        public ActionResult Create()
        {
            Activity model = new Activity();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Activity model)
        {
            if (ModelState.IsValid)
            {
                model.CreateDate = DateTime.Now;
                db.activity.Add(model);
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

        public ActionResult Edit(int sqno)
        {
            Activity model = db.activity.Where(x => x.sqno == sqno).FirstOrDefault();
            if (model == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Edit(Activity model)
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

        public ActionResult SignUpList(int sqno)
        {
            ActivitySignUpListViewModel model = new ActivitySignUpListViewModel();
            model.activitySqno = sqno;
            return SignUpList(model);
        }

        [HttpPost]
        public ActionResult SignUpList(ActivitySignUpListViewModel model)
        {
            model.activity = db.activity.Where(x => x.sqno == model.activitySqno).FirstOrDefault();
            IQueryable<ActivitySignUp> query = db.activitySignUp.Where(x => x.activitysqno == model.activitySqno);
            if (model.searchText != null && model.searchText.Trim().Length > 0)
            {
                query = query.Where(x => 
                    x.name.Contains(model.searchText) || 
                    x.email1.Contains(model.searchText) ||
                    x.email2.Contains(model.searchText) ||
                    x.mobile.Contains(model.searchText) ||
                    x.phone.Contains(model.searchText) );
            }
            model.activitySignUpPagedList = query.OrderBy(x => x.sqno).ToPagedList(model.pageNumber - 1, model.pageSize);
            return View(model);
        }

        public ActionResult DownloadReport(ActivitySignUpListViewModel model)
        {
            model.activity = db.activity.Where(x => x.sqno == model.activitySqno).FirstOrDefault();
            IQueryable<ActivitySignUp> query = db.activitySignUp.Where(x => x.activitysqno == model.activitySqno);
            if (model.searchText != null && model.searchText.Trim().Length > 0)
            {
                query = query.Where(x =>
                    x.name.Contains(model.searchText) ||
                    x.email1.Contains(model.searchText) ||
                    x.email2.Contains(model.searchText) ||
                    x.mobile.Contains(model.searchText) ||
                    x.phone.Contains(model.searchText));
            }
            List<ActivitySignUp> list = query.OrderBy(x => x.sqno).ToList();
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                ActivitySignUpListReport report = new ActivitySignUpListReport();
                IWorkbook wb = report.create(list);
                wb.Write(memoryStream);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "報名列表.xls");
        }

        public ActionResult EditActivitySignUp(int sqno)
        {
            ActivitySignUp model = db.activitySignUp.Where(x => x.sqno == sqno).FirstOrDefault();
            if (model == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.roleList = DropDownListHelper.getRoleList(false);
                ViewBag.educationLevelList = DropDownListHelper.getEducationLevelList(false);
                ViewBag.industryTypeList = DropDownListHelper.getIndustryTypeList(false);
                ViewBag.cultureList = DropDownListHelper.getCultureList(true);
                ViewBag.willComeList = DropDownListHelper.getWillComeList(false);
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult EditActivitySignUp(ActivitySignUp model)
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
    }
}
