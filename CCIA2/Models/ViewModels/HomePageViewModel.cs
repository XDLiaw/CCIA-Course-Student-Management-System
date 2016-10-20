using MvcPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCIA2.Models.ViewModels
{
    public class HomePageViewModel
    {
        public string selectedTabLinkId { get; set; }

        public string selectedTabId { get; set; }

        public string tabId { get; set; }

        public BannerListViewModel bannerListViewModel { get; set; }
        public RelativeLinkViewModel relativeLinkViewModel { get; set; }
        public BrochureViewModel brochureViewModel { get; set; }
        public AnnouncementViewModel announcementViewModel { get; set; }

        public HomePageViewModel()
        {
            this.bannerListViewModel = new BannerListViewModel();
            this.relativeLinkViewModel = new RelativeLinkViewModel();
            this.brochureViewModel = new BrochureViewModel();
            this.announcementViewModel = new AnnouncementViewModel();

            this.selectedTabLinkId = BannerListViewModel.tabLinkId;
            this.selectedTabId = BannerListViewModel.tabId;
        }
    }    

    public class BannerListViewModel
    {
        public const string tabLinkId = "tabLink-banner";
        public const string tabId = "tab-banner";

        [Display(Name = "頁碼")]
        public int pageNumber { get; set; }

        [Display(Name = "每頁資料筆數")]
        public int pageSize { get; private set; }

        // ================================================================================

        public IPagedList<BannerAndLink> bannerPagedList { get; set; }

        public BannerListViewModel()
        {
            this.pageNumber = 1;
            this.pageSize = 10;
            this.bannerPagedList = new PagedList<BannerAndLink>(null, this.pageNumber - 1, this.pageSize);
        }
    }

    public class RelativeLinkViewModel
    {
        public const string tabLinkId = "tabLnk-relativeLink";
        public const string tabId = "tab-relativeLink";

        [Display(Name = "頁碼")]
        public int pageNumber { get; set; }

        [Display(Name = "每頁資料筆數")]
        public int pageSize { get; private set; }

        // ================================================================================

        public IPagedList<BannerAndLink> relativeLinkPagedList { get; set; }

        public RelativeLinkViewModel()
        {
            this.pageNumber = 1;
            this.pageSize = 10;
            this.relativeLinkPagedList = new PagedList<BannerAndLink>(null, this.pageNumber-1, this.pageSize);
        }
    }

    public class BrochureViewModel
    {
        public const string tabLinkId = "tabLnk-brochure";
        public const string tabId = "tab-brochure";

        [Display(Name = "頁碼")]
        public int pageNumber { get; set; }

        [Display(Name = "每頁資料筆數")]
        public int pageSize { get; private set; }

        // ================================================================================

        public IPagedList<BrochureAndAnnouncement> pagedList { get; set; }

        public BrochureViewModel()
        {
            this.pageNumber = 1;
            this.pageSize = 10;
            this.pagedList = new PagedList<BrochureAndAnnouncement>(null, this.pageNumber - 1, this.pageSize);
        }
    }

    public class AnnouncementViewModel
    {
        public const string tabLinkId = "tabLnk-announcement";
        public const string tabId = "tab-announcement";

        [Display(Name = "頁碼")]
        public int pageNumber { get; set; }

        [Display(Name = "每頁資料筆數")]
        public int pageSize { get; private set; }

        // ================================================================================

        public IPagedList<BrochureAndAnnouncement> pagedList { get; set; }

        public AnnouncementViewModel()
        {
            this.pageNumber = 1;
            this.pageSize = 10;
            this.pagedList = new PagedList<BrochureAndAnnouncement>(null, this.pageNumber - 1, this.pageSize);
        }
    }


}