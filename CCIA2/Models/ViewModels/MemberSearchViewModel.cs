using MvcPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCIA2.Models.ViewModels
{
    public class MemberSearchViewModel
    {
        [Display(Name = "會員編號, 姓名, Email, 或身分證字號")]
        public string searchText { get; set; }

        [Display(Name = "會員角色")]
        public int memberTypeNo { get; set; }

        [Display(Name = "階段")]
        public int? step { get; set; }

        [Display(Name="組別")]
        public string group { get; set; }

        [Display(Name="全部/正取/備取")]
        public string enrollType { get; set; }

        [Display(Name = "頁碼")]
        public int pageNumber { get; set; }

        [Display(Name = "每頁資料筆數")]
        public int pageSize { get; private set; }

        // ---------------------------------------------------------------------------------------

        public IPagedList<Member> memberPagedList { get; set; }

        public MemberSearchViewModel()
        {
            this.pageNumber = 1;
            this.pageSize = 15;
        }
    }

    public class MemberSearchCondition {
        [Display(Name = "會員編號, 姓名, Email, 或身分證字號")]
        public string searchText { get; set; }

        [Display(Name = "會員角色")]
        public int memberTypeNo { get; set; }

        [Display(Name = "階段")]
        public int? step { get; set; }

        [Display(Name = "組別")]
        public string group { get; set; }

        [Display(Name = "全部/正取/備取")]
        public string enrollType { get; set; }
    }
}