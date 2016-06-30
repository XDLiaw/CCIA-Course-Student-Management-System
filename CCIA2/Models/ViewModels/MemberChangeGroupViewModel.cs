using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCIA2.Models.ViewModels
{
    public class MemberChangeGroupViewModel
    {
        public int sqno { get; set; }

        public Member member { get; set; }

        [Display(Name = "資格審組別")]
        public string group { get; set; }
    }
}