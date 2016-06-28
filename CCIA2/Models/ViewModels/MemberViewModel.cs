using MvcPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCIA2.Models.ViewModels
{
    public class MemberViewModel
    {

        public int memberTypeNo { get; set; }

        [Display(Name = "狀態")]
        public int? state { get; set; }

        [Display(Name="已啟動")]
        public bool isActive { get; set; }

        [Display(Name = "已完成")]
        public bool isFinish { get; set; }

        [Display(Name = "正取")]
        public bool admission { get; set; }

        [Display(Name = "備取")]
        public bool onWaitingList { get; set; }

        [Display(Name = "未錄取")]
        public bool flunk { get; set; }

        public string searchText { get; set; }

        [Display(Name = "頁碼")]
        public int pageNumber { get; set; }

        [Display(Name = "每頁資料筆數")]
        public int pageSize { get; private set; }

        // ---------------------------------------------------------------------------------------

        public IPagedList<Member> memberPagedList { get; set; }

        public MemberViewModel()
        {
            this.pageNumber = 1;
            this.pageSize = 15;
        }
    }
}