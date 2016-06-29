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
    [SessionExpire]
    public class MemberController : Controller
    {
        private CCIAContext db = new CCIAContext();

        
        public ActionResult Index()
        {
            MemberViewModel model = new MemberViewModel();
            model.memberTypeNo = 1;
            model.state = null;
            model.isActive = true;
            model.isFinish = true;
            model.admission = true;
            model.onWaitingList = true;
            model.flunk = true;
            model.searchText = null;
            return Index(model);
        }

        [HttpPost]
        public ActionResult Index(MemberViewModel model)
        {
            if (model.memberTypeNo == 1) //經紀仲介學員
            {
                IQueryable<Member> memberQuery = db.Member                 
                    .Where(m => m.mrMemberTypesqno == model.memberTypeNo)
                    .Where(m => model.isActive ? m.mrIsActive == "Y" : m.mrIsActive != "Y")
                    .Where(m => model.isFinish ? m.mrIsFinish == "Y" : m.mrIsFinish != "Y")
                    .Where(m => model.searchText == null ? true : m.mrName.Contains(model.searchText)
                        || model.searchText == null ? true : m.mrMainEmail.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrOtherEmail.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrNumber.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrId.Equals(model.searchText));
                if (model.state == 0) //待審核 
                { 
                    memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count() == 0);
                } 
                else if (model.state == 1 || model.state == 2 || model.state == 4)  //通過資格審 or 通過初審 or 已通過
                {
                    memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.state) == 0 && 
                        m.MemberGroupResult.Count(res => res.AppraiseStep == model.state && res.AppraiseResult != "0") == 1);
                }
                else if (model.state == 3) //正取/備取階段 TODO
                {
                    memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.state) == 0)
                        .Where(m => 
                            m.MemberGroupResult.Count(res => res.AppraiseStep == model.state && (model.admission ? res.AppraiseResult == "1" : false)) == 1 || 
                            m.MemberGroupResult.Count(res => res.AppraiseStep == model.state && (model.onWaitingList ? res.AppraiseResult == "2" : false)) == 1 ||
                            m.MemberGroupResult.Count(res => res.AppraiseStep == model.state && (model.flunk ? res.AppraiseResult == "0" : false)) == 1
                        );
                }
                else if (model.state == 5) { //未通過
                    memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseResult == "0") == 1);
                }                
      
                model.memberPagedList = memberQuery.OrderByDescending(r => r.mrCreateDt).ToPagedList(model.pageNumber - 1, model.pageSize);
                // 重新排序各會員的歷史審查紀錄
                foreach (Member m in model.memberPagedList)
                {
                    m.MemberGroupResult = m.MemberGroupResult.OrderBy(r => r.AppraiseStep).ToList();
                }
            }
            else if (model.memberTypeNo == 2 || model.memberTypeNo == 3) //歷屆會員 or 一般會員
            {
                model.memberPagedList = db.Member
                    .Where(m => m.mrMemberTypesqno == model.memberTypeNo)
                    .Where(m => model.isActive ? m.mrIsActive == "Y" : m.mrIsActive != "Y")
                    .Where(m => model.isFinish ? m.mrIsFinish == "Y" : m.mrIsFinish != "Y")
                    .Where(m => model.searchText == null ? true : m.mrName.Contains(model.searchText)
                        || model.searchText == null ? true : m.mrMainEmail.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrOtherEmail.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrNumber.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrId.Equals(model.searchText))
                    .OrderByDescending(r => r.mrCreateDt)
                    .ToPagedList(model.pageNumber - 1, model.pageSize);
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
                var user = Session[SessionKey.USER] as SysUser;
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
