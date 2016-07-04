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
                model.group = user.group;
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
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step && res.AppraiseNo == user.accountNo) == 0) // 表自己還沒評初審過
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1); // 已有"通過資格審"資料一筆
                        if (model.group != null && model.group.Trim().Length != 0)
                        {
                            memberQuery = memberQuery.Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 1 && res.AppraiseGroup == model.group) > 0);
                        }
                        model.memberPagedList = memberQuery.OrderBy(m => m.mrNumber).ToPagedList(model.pageNumber - 1, model.pageSize);
                    }
                    else if (model.step == 2) // 完成初審
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0) // 還沒有下一階段"進行複審"的資料
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step && res.AppraiseNo == user.accountNo) == 1); // 有一筆自己審過的初審資料
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
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step && res.AppraiseNo == user.accountNo) == 0) // 表示自己還沒評過複審
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step) == 1); // 已有一筆"進行複審"的資料 (已在複審待審核清單中)
                        model.memberPagedList = memberQuery
                            .OrderByDescending(m => m.MemberGroupResult.Where(res => res.AppraiseStep == 2).FirstOrDefault().AppraiseScore)
                            .ToPagedList(model.pageNumber - 1, model.pageSize);

                        //TODO 要測試不同外審人員是否可看到
                    }
                    else if (model.step == 4) // 完成複審
                    {
                        memberQuery = memberQuery
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > model.step) == 0) // 還沒有下一階段的資料
                            .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == model.step && res.AppraiseNo == user.accountNo) == 1); // 已有一筆自己審過的複審資料
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
                        if (model.enrollType != null && model.enrollType.Trim().Length != 0)
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
                        if (model.enrollType != null && model.enrollType.Trim().Length != 0)
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
            ViewBag.enrollTypeList = DropDownListHelper.getEnrollTypeList(true);
            return View("Index", model);
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

        // 資格審
        public ActionResult QualificationVerify(int sqno)
        {
            MemberQualificationVerifyViewModel model = new MemberQualificationVerifyViewModel();
            model.sqno = sqno;
            model.member = db.Member.Where(m => m.sqno == sqno && m.MemberGroupResult.Count() == 0).FirstOrDefault();
            if (model.member == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
                return Index();
            }

            ViewBag.groupList = DropDownListHelper.getAppraiseGroupNameList(false);
            return View(model);
        }

        // 資格審
        [HttpPost]
        public ActionResult QualificationVerify(MemberQualificationVerifyViewModel model)
        {
            try
            {
                model.member = db.Member.Where(m => m.sqno == model.sqno && m.MemberGroupResult.Count() == 0).FirstOrDefault();
                if (model.member == null)
                {
                    ViewBag.ErrorMessage = "找不到資料";
                    return Index();
                }
                if (ModelState.IsValid)
                {
                    MemberGroupResult newResult = new MemberGroupResult(model.member);
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

                    db.Entry(model.member).State = EntityState.Modified;
                    db.SaveChanges();
                    return View("Close");
                }
                ViewBag.groupList = DropDownListHelper.getAppraiseGroupNameList(false);
                return View(model);
            }
            catch (Exception e)
            {
                return QualificationVerify(model.sqno);
            }
        }

        // 變更資格審組別
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

        // 變更資格審組別
        [HttpPost]
        public ActionResult ChangeGroup(MemberChangeGroupViewModel model)
        {
            model.member = db.Member.Where(m => m.sqno == model.sqno).FirstOrDefault();
            if (ModelState.IsValid)
            {
                MemberGroupResult result = model.member.MemberGroupResult.Where(res => res.AppraiseStep == 1).FirstOrDefault();
                result.AppraiseGroup = model.group;
                db.Entry(model.member).State = EntityState.Modified;
                db.SaveChanges();
                return View("Close");
            }
            ViewBag.groupList = DropDownListHelper.getAppraiseGroupNameList(false);
            return View(model);
        }

        // 初審
        public ActionResult FirstTrail(int sqno)
        {
            SysUser user = Session[SessionKey.USER] as SysUser;
            MemberTrailViewModel model = new MemberTrailViewModel();
            model.sqno = sqno;
            model.member = db.Member
                .Where(m => m.sqno == sqno)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 1 && res.AppraiseNo == user.accountNo) == 0)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 1) == 1)
                .FirstOrDefault();
            if (model.member == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
                return Index();
            }
            return View(model);
        }

        // 初審
        [HttpPost]
        public ActionResult FirstTrail(MemberTrailViewModel model)
        {
            SysUser user = Session[SessionKey.USER] as SysUser;
            model.member = db.Member
                .Where(m => m.sqno == model.sqno)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 1 && res.AppraiseNo == user.accountNo) == 0)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 1) == 1)
                .FirstOrDefault();
            if (model.member == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
                return Index();
            }
            if (ModelState.IsValid)
            {
                MemberGroupResult newResult = new MemberGroupResult(model.member);
                newResult.AppraiseStep = 2;
                newResult.AppraiseState = "完成初審";
                newResult.AppraiseScore = model.score;
                newResult.AppraiseDesc = model.desc;

                db.Entry(model.member).State = EntityState.Modified;
                db.SaveChanges();
                return View("Close");
            }
            return View(model);
        }

        // 初審直接正取
        public ActionResult Admission(int sqno)
        {
            SysUser user = Session[SessionKey.USER] as SysUser;
            Member member = db.Member
                .Where(m => m.sqno == sqno)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 2) == 0)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 2 && res.AppraiseNo == user.accountNo) == 1)
                .FirstOrDefault();
            if (member == null)
            {
                var result = new
                {
                    success = false,
                    errorMessage = "找不到資料"
                };
                return Json(result);
            }
            else
            {
                try
                {
                    MemberGroupResult newResult = new MemberGroupResult(member);
                    newResult.AppraiseStep = 5;
                    newResult.AppraiseState = "正取";
                    newResult.AppraiseResult = "1";
                    newResult.AppraiseGroup = member.MemberGroupResult.Where(res => res.AppraiseStep == 1).FirstOrDefault().AppraiseGroup;

                    db.Entry(member).State = EntityState.Modified;
                    db.SaveChanges();

                    var result = new { success = true, message = "已將會員 "+member.mrName+" 加入正取名單!" };
                    return Json(result);
                }
                catch (Exception e)
                {
                    var result = new { success = false, errorMessages = e.Message };
                    return Json(result);
                }
            }
        }

        // 進入複審
        public ActionResult IntoSecondTrail(int sqno)
        {
            SysUser user = Session[SessionKey.USER] as SysUser;
            Member member = db.Member
                .Where(m => m.sqno == sqno)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 2) == 0)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 2 && res.AppraiseNo == user.accountNo) == 1)
                .FirstOrDefault();
            if (member == null)
            {
                var result = new
                {
                    success = false,
                    errorMessage = "找不到資料"
                };
                return Json(result);
            }
            else
            {
                try
                {
                    MemberGroupResult newResult = new MemberGroupResult(member);
                    newResult.AppraiseStep = 3;
                    newResult.AppraiseState = "進行複審";
                    db.Entry(member).State = EntityState.Modified;
                    db.SaveChanges();

                    var result = new { success = true, message = "已將會員 " + member.mrName + " 加入複審名單!" };
                    return Json(result);
                }
                catch (Exception e)
                {
                    var result = new { success = false, errorMessages = e.Message };
                    return Json(result);
                }
            }
        }

        // 未錄取
        public ActionResult Flunk(int sqno)
        {
            Member member = db.Member
                .Where(m => m.sqno == sqno)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 7) == 0)
                .FirstOrDefault();
            if (member == null)
            {
                var result = new
                {
                    success = false,
                    errorMessage = "找不到資料"
                };
                return Json(result);
            }
            else
            {
                try
                {
                    member.MemberGroupResult = member.MemberGroupResult.OrderBy(res => res.AppraiseStep).ToList();
                    if (member.MemberGroupResult.LastOrDefault().AppraiseStep == 2)
                    {
                        MemberGroupResult newResult = new MemberGroupResult(member);
                        newResult.AppraiseStep = 7;
                        newResult.AppraiseState = "初審未錄取";
                    }
                    else if (member.MemberGroupResult.LastOrDefault().AppraiseStep == 4)
                    {
                        MemberGroupResult newResult = new MemberGroupResult(member);
                        newResult.AppraiseStep = 7;
                        newResult.AppraiseState = "複審未錄取";
                    }

                    db.Entry(member).State = EntityState.Modified;
                    db.SaveChanges();

                    var result = new { success = true, message = "已將會員 " + member.mrName + " 加入未錄取名單!" };
                    return Json(result);
                }
                catch (Exception e)
                {
                    var result = new { success = false, errorMessages = e.Message };
                    return Json(result);
                }
            }
        }

        // 複審
        public ActionResult SecondTrail(int sqno)
        {
            SysUser user = Session[SessionKey.USER] as SysUser;
            MemberTrailViewModel model = new MemberTrailViewModel();
            model.sqno = sqno;
            model.member = db.Member
                .Where(m => m.sqno == sqno)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 3 && res.AppraiseNo == user.accountNo) == 0)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 3) == 1)
                .FirstOrDefault();
            if (model.member == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
                return Index();
            }
            return View(model);
        }

        // 複審
        [HttpPost]
        public ActionResult SecondTrail(MemberTrailViewModel model)
        {
            SysUser user = Session[SessionKey.USER] as SysUser;
            model.member = db.Member
                .Where(m => m.sqno == model.sqno)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 3 && res.AppraiseNo == user.accountNo) == 0)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 3) == 1)
                .FirstOrDefault();
            if (model.member == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
                return Index();
            }
            if (ModelState.IsValid)
            {
                MemberGroupResult newResult = new MemberGroupResult(model.member);
                newResult.AppraiseStep = 4;
                newResult.AppraiseState = "完成複審";
                newResult.AppraiseScore = model.score;
                newResult.AppraiseDesc = model.desc;

                db.Entry(model.member).State = EntityState.Modified;
                db.SaveChanges();
                return View("Close");
            }
            return View(model);
        }

        public ActionResult Admit(int sqno)
        {
            SysUser user = Session[SessionKey.USER] as SysUser;
            MemberAdmitViewModel model = new MemberAdmitViewModel();
            model.sqno = sqno;
            model.member = db.Member
                .Where(m => m.sqno == sqno)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 4) == 0)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 4) > 0)
                .FirstOrDefault();
            if (model.member == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
                return Index();
            }
            ViewBag.enrollTypeList = DropDownListHelper.getEnrollTypeList(false);
            ViewBag.groupList = DropDownListHelper.getAppraiseGroupNameList(false);
            return View(model);
        }

        [HttpPost]
        public ActionResult Admit(MemberAdmitViewModel model)
        {
            model.member = db.Member
                .Where(m => m.sqno == model.sqno)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 4) == 0)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 4) > 0)
                .FirstOrDefault();
            if (model.member == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
                return Index();
            }
            if (ModelState.IsValid)
            {
                MemberGroupResult newResult = new MemberGroupResult(model.member);
                newResult.AppraiseStep = 5;
                if (model.result == "1")
                {
                    newResult.AppraiseState = "正取";
                }
                if (model.result == "2")
                {
                    newResult.AppraiseState = "備取";
                }
                newResult.AppraiseResult = model.result;
                newResult.AppraiseGroup = model.group;

                db.Entry(model.member).State = EntityState.Modified;
                db.SaveChanges();
                return View("Close");
            }
            ViewBag.enrollTypeList = DropDownListHelper.getEnrollTypeList(false);
            ViewBag.groupList = DropDownListHelper.getAppraiseGroupNameList(false);
            return View(model);
        }

        // 繳交保證金
        public ActionResult PayDeposit(int sqno, bool hasPaiedDeposit)
        {
            Member member = db.Member
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep > 5) == 0)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5) == 1)
                .FirstOrDefault();
            if (member == null)
            {
                var result = new
                {
                    success = false,
                    errorMessage = "找不到資料"
                };
                return Json(result);
            }
            else
            {
                try
                {
                    MemberGroupResult newResult = new MemberGroupResult(member);
                    string msg;
                    if (hasPaiedDeposit)
                    {
                        newResult.AppraiseStep = 6;
                        newResult.AppraiseState = "已通過";
                        msg = "會員 " + member.mrName + " 已繳保證金!";
                    }
                    else
                    {
                        newResult.AppraiseStep = 7;
                        newResult.AppraiseState = "未繳交保證金";
                        msg = "會員 " + member.mrName + " 未繳保證金!";
                    }
                    db.Entry(member).State = EntityState.Modified;
                    db.SaveChanges();

                    var result = new { success = true, message = msg };
                    return Json(result);
                }
                catch (Exception e)
                {
                    var result = new { success = false, errorMessages = e.Message };
                    return Json(result);
                }
            }
        }

        // 轉為一般會員
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
                        return new FileContentResult(bytes, contentType)
                        {
                            FileDownloadName = attachF.mrShowFileName
                        };
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
