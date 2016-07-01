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
            SysUser user = Session[SessionKey.USER] as SysUser;
            MemberViewModel model = new MemberViewModel();
            if (user.role == 1 || user.role == 3)
            {
                model.memberTypeNo = 1;
                model.step = null;
                model.searchText = null;
            }
            else if (user.role == 2)
            {
                model.memberTypeNo = 1;
                model.step = 1;
                model.searchText = null;
            }

            return Index(model);
        }

        [HttpPost]
        public ActionResult Index(MemberViewModel model)
        {
            SysUser user = Session[SessionKey.USER] as SysUser;
            if (model.memberTypeNo == 1) //經紀仲介學員
            {
                IQueryable<Member> memberQuery = db.Member.Where(m => m.mrMemberTypesqno == model.memberTypeNo && m.mrIsActive == "Y" && m.mrIsFinish == "Y");
                if (model.searchText != null && model.searchText.Trim().Length > 0) //只要這內容不為空就忽略其他條件
                {
                    memberQuery = memberQuery
                        .Where(m => m.mrName.Contains(model.searchText)
                            || m.mrMainEmail.Equals(model.searchText)
                            || m.mrOtherEmail.Equals(model.searchText)
                            || m.mrNumber.Equals(model.searchText)
                            || m.mrId.Equals(model.searchText));
                    model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                }
                else
                {
                    if (model.step == null) // 全部階段
                    {
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 0) // 待審核 
                    {
                        memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count() == 0);
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 1)  // 通過資格審
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0)
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1);
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 1 && res.AppraiseGroup == model.group) > 0);
                        }
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 2) // 完成初審
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0)
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1);
                        if (model.group != null && model.group.Trim().Length != 0) {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 1 && res.AppraiseGroup == model.group) > 0);
                        }
                        model.memberPagedList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 2).FirstOrDefault().AppraiseScore)
                            .ToPagedList(model.pageNumber - 1, model.pageSize);   
                    }
                    else if (model.step == 3) // 進行複審
                    {                        
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step && res.AppraiseNo == user.accountNo) == 0)
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1);
                        model.memberPagedList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 2).FirstOrDefault().AppraiseScore)
                            .ToPagedList(model.pageNumber - 1, model.pageSize);

                        //TODO 要測試不同外審人員是否可看到
                    }
                    else if (model.step == 4) // 完成複審
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0) 
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) > 0);
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 1 && res.AppraiseGroup == model.group) > 0);
                        }
                        model.memberPagedList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == model.step).Average(res => res.AppraiseScore))
                            .ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 5) // 正/備取名單
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0)
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1);
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseGroup == model.group) > 0);
                        }
                        if (model.enrollType != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseResult == model.enrollType) > 0);
                        }
                            
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 6) //已通過
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0)
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1);
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseGroup == model.group) > 0);
                        }
                        if (model.enrollType != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseResult == model.enrollType) > 0);
                        }
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 7) //未通過
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0)
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1);
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                }
                    
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
                    .Where(m => model.searchText == null ? true : m.mrName.Contains(model.searchText)
                        || model.searchText == null ? true : m.mrMainEmail.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrOtherEmail.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrNumber.Equals(model.searchText)
                        || model.searchText == null ? true : m.mrId.Equals(model.searchText))
                    .OrderBy(m => m.mrNumber)
                    .ToPagedList(model.pageNumber - 1, model.pageSize);
            }

            ViewBag.stepList = DropDownListHelper.getApplyStepListWithAll();
            ViewBag.groupList = DropDownListHelper.getAppraiseGroupNameList(true);
            ViewBag.enrollTypeList = DropDownListHelper.getEnrollTypeList();
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

        //資格審
        public ActionResult QualificationVerify(int sqno)
        {
            MemberQualificationVerifyViewModel model = new MemberQualificationVerifyViewModel();
            model.sqno = sqno;
            model.member = db.Member.Where(m => m.sqno == sqno).FirstOrDefault();
            if (model.member == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
            }
            ViewBag.groupList = DropDownListHelper.getAppraiseGroupNameList(false);
            return View(model);
        }

        [HttpPost]
        public ActionResult QualificationVerify(MemberQualificationVerifyViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.member = db.Member.Where(m => m.sqno == model.sqno).FirstOrDefault();
                MemberGroupResult newResult = new MemberGroupResult();
                newResult.mrSqno = model.member.sqno;
                newResult.mrNumber = model.member.mrNumber;
                newResult.member = model.member;
                if (model.isPass)
                {
                    newResult.AppraiseStep = 1;
                    newResult.AppraiseGroup = model.group;
                    newResult.AppraiseState = "通過資格審";
                    newResult.AppraiseDesc = model.desc;
                }
                else
                {
                    newResult.AppraiseStep = 7;
                    newResult.AppraiseState = "資格審未錄取";
                    newResult.AppraiseDesc = model.desc;
                }
                newResult.AppraiseCreateDt = DateTime.Now;
                SysUser user = Session[SessionKey.USER] as SysUser;
                newResult.AppraiseNo = user.accountNo;
                model.member.MemberGroupResult.Add(newResult);

                db.Entry(model.member).State = EntityState.Modified;
                db.SaveChanges();
                return View("Close");
            }
            ViewBag.groupList = DropDownListHelper.getAppraiseGroupNameList(false);
            return View(model);
        }

        public ActionResult ChangeGroup(int sqno)
        {
            MemberChangeGroupViewModel model = new MemberChangeGroupViewModel();
            model.sqno = sqno;
            model.member = db.Member.Where(m => m.sqno == sqno).FirstOrDefault();
            if (model.member == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
            }
            ViewBag.groupList = DropDownListHelper.getAppraiseGroupNameList(false);
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeGroup(MemberChangeGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.member = db.Member.Where(m => m.sqno == model.sqno).FirstOrDefault();
                MemberGroupResult result = model.member.MemberGroupResult.Where(res => res.AppraiseStep == 1).FirstOrDefault();
                result.AppraiseGroup = model.group;
                db.Entry(model.member).State = EntityState.Modified;
                db.SaveChanges();
                return View("Close");
            }
            ViewBag.groupList = DropDownListHelper.getAppraiseGroupNameList(false);
            return View(model);
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
                ViewBag.AppraiseGroupList = DropDownListHelper.getAppraiseGroupNameList(false);
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
            ViewBag.AppraiseGroupList = DropDownListHelper.getAppraiseGroupNameList(false);
            return View(model);
        }

        public ActionResult convertToGeneralMember(int memberSqno)
        {
            Member member = db.Member.Where(m => m.sqno == memberSqno).FirstOrDefault();
            if (member == null)
            {
                var result = new
                {
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
