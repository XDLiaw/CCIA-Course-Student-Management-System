using CCIA2.Models;
using CCIA2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcPaging;
using CCIA2.Services;

namespace CCIA2.Controllers
{
    [Authorize]
    [SessionExpire]
    public class HomeController : Controller
    {
        private CCIAContext db = new CCIAContext();
        //
        // GET: /Home/

        #region index

        public ActionResult Index()
        {
            return Index(new HomePageViewModel());
        }

        [HttpPost]
        public ActionResult Index(HomePageViewModel model)
        {
            if (Session[SessionKey.USER] == null)
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("Login", "Account");
            }

            if (model.selectedTabId == BannerListViewModel.tabId)
            {
                model.bannerListViewModel.bannerPagedList = SearchBanner(model.bannerListViewModel);
            }
            if (model.selectedTabId == RelativeLinkViewModel.tabId)
            {
                model.relativeLinkViewModel.relativeLinkPagedList = SearchRelativeLink(model.relativeLinkViewModel);
            }
            if (model.selectedTabId == BrochureViewModel.tabId)
            {
                model.brochureViewModel.pagedList = searchBrochure(model.brochureViewModel);
            }
            if (model.selectedTabId == AnnouncementViewModel.tabId)
            {
                model.announcementViewModel.pagedList = searchAnnouncement(model.announcementViewModel);
            }

            return View(model);
        }

        private IPagedList<BannerAndLink> SearchBanner(BannerListViewModel model)
        {
            IPagedList<BannerAndLink> list = db.bannerAndLink
                .Where(x => x.type == BannerAndLink.TYPE_BANNER)
                .OrderByDescending(x => x.createDate)
                .ToPagedList(model.pageNumber - 1, model.pageSize);
            return list;
        }

        private IPagedList<BannerAndLink> SearchRelativeLink(RelativeLinkViewModel model)
        {
            IPagedList<BannerAndLink> list = db.bannerAndLink
                .Where(x => x.type == BannerAndLink.TYPE_LINK)
                .OrderByDescending(x => x.createDate)
                .ToPagedList(model.pageNumber - 1, model.pageSize);
            return list;
        }

        private IPagedList<BrochureAndAnnouncement> searchBrochure(BrochureViewModel model)
        {
            IPagedList<BrochureAndAnnouncement> list = db.brochureAndAnnouncement
                .Where(x => x.type == BrochureAndAnnouncement.TYPE_BROCHURE)
                .OrderByDescending(x => x.createDate)
                .ToPagedList(model.pageNumber - 1, model.pageSize);
            return list;
        }

        private IPagedList<BrochureAndAnnouncement> searchAnnouncement(AnnouncementViewModel model)
        {
            IPagedList<BrochureAndAnnouncement> list = db.brochureAndAnnouncement
                .Where(x => x.type == BrochureAndAnnouncement.TYPE_ANNOUNCEMENT)
                .OrderByDescending(x => x.createDate)
                .ToPagedList(model.pageNumber - 1, model.pageSize);
            return list;
        }

        #endregion

        #region Banner 和 相關連結

        public ActionResult CreateBannerAndLink(string type)
        {
            BannerAndLink model = new BannerAndLink();
            model.type = type;
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateBannerAndLink(BannerAndLink model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    new BannerAndLinkService(db).create(model);
                    return Json(new { success = true, message = "新增成功" });
                }
                catch (Exception e)
                {
                    return Json(new { success = false, errorMessage = e.Message });
                }
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

        public ActionResult EditBannerAndLink(int sqno)
        {
            BannerAndLink model = db.bannerAndLink.Where(x => x.sqno == sqno).FirstOrDefault();
            if (model == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditBannerAndLink(BannerAndLink model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    new BannerAndLinkService(db).edit(model);
                    return Json(new { success = true, message = "編輯成功" });
                }
                catch (Exception e)
                {
                    return Json(new { success = false, errorMessage = e.Message });
                }
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

        #region 簡章 和 注意事項

        public ActionResult CreateBrochureAndAnnouncement(string type)
        {
            BrochureAndAnnouncement model = new BrochureAndAnnouncement();
            model.type = type;
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateBrochureAndAnnouncement(BrochureAndAnnouncement model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    new BrochureAndAnnouncementService(db).create(model);
                    return Json(new { success = true, message = "新增成功" });
                }
                catch (Exception e)
                {
                    return Json(new { success = false, errorMessage = e.Message });
                }
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

        public ActionResult EditBrochureAndAnnouncement(int sqno)
        {
            BrochureAndAnnouncement model = db.brochureAndAnnouncement.Where(x => x.sqno == sqno).FirstOrDefault();
            if (model == null)
            {
                ViewBag.ErrorMessage = "找不到資料";
            }
            return View(model);
        }
        
        [HttpPost]
        public ActionResult EditBrochureAndAnnouncement(BrochureAndAnnouncement model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    new BrochureAndAnnouncementService(db).edit(model);
                    return Json(new { success = true, message = "編輯成功" });
                }
                catch (Exception e)
                {
                    return Json(new { success = false, errorMessage = e.Message });
                }
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

        public FileContentResult DownloadDbFile(int sqno)
        {
            DbFile dbFile = db.dbFile.Where(x => x.sqno == sqno).FirstOrDefault();
            return dbFile == null ? null : new FileContentResult(dbFile.content, dbFile.contentType);
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
