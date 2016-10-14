using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCIA2.Models.ViewModels
{
    public class CourseExamResultViewModel
    {
        public Member student { get; set; }

        public List<MemberQAnswer> qaList { get; set; }

        
    }
}