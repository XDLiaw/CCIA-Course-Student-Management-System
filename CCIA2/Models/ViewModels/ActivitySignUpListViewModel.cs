using MvcPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCIA2.Models.ViewModels
{
    public class ActivitySignUpListViewModel
    {
        public ActivitySignUpListViewModel()
        {
            this.pageNumber = 1;
            this.pageSize = 15;
        }

        [Display(Name = "頁碼")]
        public int pageNumber { get; set; }

        [Display(Name = "每頁資料筆數")]
        public int pageSize { get; private set; }

        public int activitySqno { get; set; }

        public string searchText { get; set; }

        // ===========================================================

        public Activity activity { get; set; }

        public IPagedList<ActivitySignUp> activitySignUpPagedList { get; set; }


    }
}