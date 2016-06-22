using CCIA2.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace CCIA2.Models.ViewModels
{
    public class MemberAppraiseViewModel
    {
        public int sqno { get; set; }

        public Member member { get; set; }

        public MemberGroupResult newAppraiseResult { get; set; }

        public List<SelectListItem> AppraiseResultList { get; set; }

    }
}