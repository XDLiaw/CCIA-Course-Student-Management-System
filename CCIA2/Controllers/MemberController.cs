using CCIA2.Helper;
using CCIA2.Models;
using CCIA2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPaging;
using System.Data.Entity;
using System.Web.Configuration;
using System.IO;

namespace CCIA2.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private CCIAContext db = new CCIAContext();

        
        public ActionResult Index()
        {
            MemberViewModel model = new MemberViewModel();
            model.memberPagedList = db.Member
                .Where(m => m.mrMemberTypesqno == 1) //經紀仲介學員
                .OrderByDescending(r => r.mrCreateDt)
                .ToPagedList(model.pageNumber - 1, model.pageSize);
            foreach (Member m in model.memberPagedList)
            {
                m.MemberGroupResult = m.MemberGroupResult.OrderBy(r => r.AppraiseStep).ToList();
            }

            ViewBag.stateList = DropDownListHelper.getApplyStepListWithAll();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(MemberViewModel model)
        {
            model.memberPagedList = db.Member
                .Where(m => m.mrMemberTypesqno == 1) //經紀仲介學員
                .Where(m => (model.state == 0) ? (m.MemberGroupResult.Count() == 0) : true)  //待審核
                .Where
                (m => (1 <= model.state && model.state <= 4) ? //已通過某階段
                    (
                        m.MemberGroupResult.Count(res => res.AppraiseStep > model.state) == 0 && 
                        m.MemberGroupResult.Count(res => res.AppraiseStep == model.state && res.AppraiseResult != "0") == 1                        
                    ) : true
                )
                .Where(m => (model.state == 5) ? (m.MemberGroupResult.Count(res => res.AppraiseResult == "0") == 1) : true) //未通過
                .OrderByDescending(r => r.mrCreateDt)
                .ToPagedList(model.pageNumber - 1, model.pageSize);
            foreach (Member m in model.memberPagedList)
            {
                m.MemberGroupResult = m.MemberGroupResult.OrderBy(r => r.AppraiseStep).ToList();
            }

            ViewBag.stateList = DropDownListHelper.getApplyStepListWithAll();
            return View(model);
        }

        public ActionResult Details(int sqno)
        {
            Member member = db.Member.Where(m => m.sqno == sqno).FirstOrDefault();
            if (member == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
                return RedirectToAction("Index");
            }
            else
            {
                return View(member);
            }
        }

        public ActionResult convertToGeneralMember(int memberSqno)
        {
            Member member = db.Member.Where(m => m.sqno == memberSqno).FirstOrDefault();
            if (member == null)
            {
                var result = new { 
                    convertSuccess = false,
                    errorMessage = "找不到資料"
                };
                return Json(result);
            }
            else
            {
                member.mrMemberTypesqno = 3;
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                var result = new { convertSuccess = true };
                return Json(result);
            }
        }

        public ActionResult Appraise(int sqno)
        {
            MemberAppraiseViewModel model = new MemberAppraiseViewModel();
            model.sqno = sqno;
            model.member =  db.Member.Where(m => m.sqno == sqno).FirstOrDefault();
            if (model.member != null)
            {
                model.newAppraiseResult = new MemberGroupResult();
                model.newAppraiseResult.mrSqno = model.member.sqno;
                model.newAppraiseResult.mrNumber = model.member.mrNumber;
                if (model.member.MemberGroupResult != null && model.member.MemberGroupResult.Count > 0)
                {
                    model.newAppraiseResult.AppraiseStep = model.member.MemberGroupResult.LastOrDefault().AppraiseStep + 1;
                }
                else
                {
                    model.newAppraiseResult.AppraiseStep = 1;
                }
                var user = Session["user"] as SysUser;
                model.newAppraiseResult.AppraiseNo = user.accountNo;

                ViewBag.AppraiseResultList = DropDownListHelper.getAppraiseResultList(model.newAppraiseResult.AppraiseStep);
                ViewBag.AppraiseGroupList = DropDownListHelper.getAppraiseGroupNameList();
                return View(model);
            }
            else
            {
                ViewBag.ErrorMessage = "找不到資料";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Appraise(MemberAppraiseViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.member = db.Member.Where(m => m.sqno == model.sqno).FirstOrDefault();
                model.newAppraiseResult.AppraiseCreateDt = DateTime.Now;
                model.member.MemberGroupResult.Add(model.newAppraiseResult);
                db.Entry(model.member).State = EntityState.Modified;
                db.SaveChanges();
                return View("Close");
            }
            ViewBag.AppraiseResultList = DropDownListHelper.getAppraiseResultList(model.newAppraiseResult.AppraiseStep);
            ViewBag.AppraiseGroupList = DropDownListHelper.getAppraiseGroupNameList();
            return View(model);
        }

        public FileContentResult DownloadMemberAttchFile(int sqno)
        {
            MemberAttchFile attachF = db.MemberAttchFile.Where(x => x.sqno == sqno).FirstOrDefault();
            if (attachF == null)
            {
                return null;
            }
            else
            {
                string folder = WebConfigurationManager.AppSettings["MemberAttchFileDir"];
                string filePath = System.IO.Path.Combine(folder, attachF.mrNumber, attachF.mrAttchFileName);
                string contentType = GetContentTypeForFileName(attachF.mrAttchFileName);
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
                        return new FileContentResult(bytes, contentType);
                    }
                }
            }
        }

        private static string GetContentTypeForFileName(string fileName)
        {
            string ext = System.IO.Path.GetExtension(fileName);
            using (Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext))
            {
                if (registryKey == null)
                    return null;
                var value = registryKey.GetValue("Content Type");
                return (value == null) ? string.Empty : value.ToString();
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
