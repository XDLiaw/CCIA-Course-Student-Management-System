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
        [Display(Name = "狀態")]
        public int? state { get; set; }

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