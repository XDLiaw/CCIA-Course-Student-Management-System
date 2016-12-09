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
using CCIA2.Services;
using CCIA2.Helper.ExcelReport;
using NPOI.SS.UserModel;

namespace CCIA2.Controllers
{
    [Authorize]
    [SessionExpire]
    public class MemberController : Controller
    {
        private CCIAContext db = new CCIAContext();
        private MemberService memberService;

        public MemberController()
        {
            this.memberService = new MemberService(this.db);
        }

        
        public ActionResult Index()
        {
            SysUser user = Session[SessionKey.USER] as SysUser;
            MemberSearchViewModel model = new MemberSearchViewModel();
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
        public ActionResult Index(MemberSearchViewModel model)
        {
            SysUser user = Session[SessionKey.USER] as SysUser;
            model = this.memberService.searchNPagging(model, user);

            ViewBag.stepList = DropDownListHelper.getApplyStepListWithAll();
            ViewBag.groupList = DropDownListHelper.getAppraiseGroupNameList(true);
            ViewBag.enrollTypeList = DropDownListHelper.getEnrollTypeList(true);
            ViewBag.andOrList = DropDownListHelper.getAndOrList();
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult DownloadReport(MemberSearchViewModel model)
        {
            SysUser user = Session[SessionKey.USER] as SysUser;
            MemoryStream memoryStream = new MemoryStream();
            try
            {                
                List<Member> memberList = this.memberService.search(model, user);
                MemberReport report = new MemberReport();
                IWorkbook wb = report.create(memberList);
                wb.Write(memoryStream);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return File(memoryStream.ToArray(), "application/vnd.ms-excel", "會員資料.xls");
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
            IQueryable<Member> memberQuery = db.Member.Where(m => m.sqno == sqno);
            model.member = this.memberService.queryMemberAtStep1(memberQuery, user).FirstOrDefault();
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
            IQueryable<Member> memberQuery = db.Member.Where(m => m.sqno == model.sqno);
            model.member = this.memberService.queryMemberAtStep1(memberQuery, user).FirstOrDefault();
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
            IQueryable<Member> memberQuery = db.Member.Where(m => m.sqno == sqno);
            Member member = this.memberService.queryMemberAtStep2(memberQuery, user).FirstOrDefault();
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
                    var result = new { success = false, errorMessage = e.Message };
                    return Json(result);
                }
            }
        }

        // 進入複審
        public ActionResult IntoSecondTrail(int sqno)
        {
            SysUser user = Session[SessionKey.USER] as SysUser;
            IQueryable<Member> memberQuery = db.Member.Where(m => m.sqno == sqno);
            Member member = this.memberService.queryMemberAtStep2(memberQuery, user).FirstOrDefault();
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
                    var result = new { success = false, errorMessage = e.Message };
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
                    else if (member.MemberGroupResult.LastOrDefault().AppraiseStep == 5)
                    {
                        MemberGroupResult newResult = new MemberGroupResult(member);
                        newResult.AppraiseStep = 7;
                        newResult.AppraiseState = "備取未通過";
                    }

                    db.Entry(member).State = EntityState.Modified;
                    db.SaveChanges();

                    var result = new { success = true, message = "已將會員 " + member.mrName + " 加入未錄取名單!" };
                    return Json(result);
                }
                catch (Exception e)
                {
                    var result = new { success = false, errorMessage = e.Message };
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
            IQueryable<Member> memberQuery = db.Member.Where(m => m.sqno == sqno);
            model.member = this.memberService.queryMemberAtStep3(memberQuery, user).FirstOrDefault();
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
            IQueryable<Member> memberQuery = db.Member.Where(m => m.sqno == model.sqno);
            model.member = this.memberService.queryMemberAtStep3(memberQuery, user).FirstOrDefault();
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
            IQueryable<Member> memberQuery = db.Member.Where(m => m.sqno == sqno);
            model.member = this.memberService.queryMemberAtStep4(memberQuery, user).FirstOrDefault();
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
            SysUser user = Session[SessionKey.USER] as SysUser;
            IQueryable<Member> memberQuery = db.Member.Where(m => m.sqno == model.sqno);
            model.member = this.memberService.queryMemberAtStep4(memberQuery, user).FirstOrDefault();
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

        // 複審備取轉正取
        public ActionResult ConvertToAdmission(int sqno)
        {
            Member member = db.Member.Where(m => m.sqno == sqno)
                .Where(m => m.MemberGroupResult.Count(res => res.AppraiseStep == 5 && res.AppraiseResult == "2") == 1)
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
                    newResult.AppraiseResult = "1";
                    newResult.AppraiseGroup = member.MemberGroupResult.Where(res => res.AppraiseStep == 5).FirstOrDefault().AppraiseGroup;
                    newResult.AppraiseState = "正取(遞補)";
                    string msg = "會員 " + member.mrName + " 已遞補為正取!";

                    db.Entry(member).State = EntityState.Modified;
                    db.SaveChanges();

                    var result = new { success = true, message = msg };
                    return Json(result);
                }
                catch (Exception e)
                {
                    var result = new { success = false, errorMessage = e.Message };
                    return Json(result);
                }
            }
        }

        // 繳交保證金
        public ActionResult PayDeposit(int sqno, bool hasPaiedDeposit)
        {
            IQueryable<Member> memberQuery = db.Member.Where(m => m.sqno == sqno);
            memberQuery = this.memberService.queryMemberAtStep5(memberQuery);
            memberQuery = this.memberService.queryMemberEnrollType(memberQuery, "1");
            Member member = memberQuery.FirstOrDefault();
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
                    var result = new { success = false, errorMessage = e.Message };
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
                string contentType = FileUtils.GetContentTypeForFileName(attachF.mrAttchFileName);
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
