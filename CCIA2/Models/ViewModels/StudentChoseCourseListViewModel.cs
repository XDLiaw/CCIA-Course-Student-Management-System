using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCIA2.Models.ViewModels
{
    public class StudentChoseCourseListViewModel
    {
        public Member student { get; set; }

        public List<MemberCourse> courseList { get; set; }
    }
}