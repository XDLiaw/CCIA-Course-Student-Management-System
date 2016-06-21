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
                .OrderBy(r => r.mrName)
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
                .OrderBy(m => m.mrName)
                .ToPagedList(model.pageNumber - 1, model.pageSize);
            foreach (Member m in model.memberPagedList)
            {
                m.MemberGroupResult = m.MemberGroupResult.OrderBy(r => r.AppraiseStep).ToList();
            }

            ViewBag.stateList = DropDownListHelper.getApplyStepListWithAll();
            return View(model);
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
