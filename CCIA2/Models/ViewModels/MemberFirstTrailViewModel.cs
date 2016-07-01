using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCIA2.Models.ViewModels
{
    public class MemberFirstTrailViewModel
    {
        public int sqno { get; set; }

        public Member member { get; set; }

        [Display(Name = "分數")]
        public double score { get; set; }

        [Display(Name = "意見")]
        public string desc { get; set; }
    }
}