using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCIA2.Models.ViewModels
{
    public class CourseStudentListViewModel
    {
        public Course course { get; set; }

        public List<MemberCourse> memberCourseList { get; set; }


    }
}