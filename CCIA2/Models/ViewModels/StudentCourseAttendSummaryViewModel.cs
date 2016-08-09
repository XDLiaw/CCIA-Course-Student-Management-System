using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCIA2.Models.ViewModels
{
    public class StudentCourseAttendSummaryViewModel
    {
        public Member student { get; set; }

        public List<MemberCourse> courseList { get; set; }

        public List<CourseGroup> nonElectiveCourseGroupList { get; set; }

        public List<CourseGroup> electiveCourseGroupList { get; set; }

        public Dictionary<CourseGroup, double> nonElectiveCourseMustAttendHourList { get; set; }

        public double electiveCourseMustAttendHour { get; set; }

        public Dictionary<CourseGroup, double> nonElectiveCourseAttendHourList { get; set; }

        public double electiveCourseAttendHour { get; set; }

        public StudentCourseAttendSummaryViewModel()
        {
            this.nonElectiveCourseAttendHourList = new Dictionary<CourseGroup, double>();
            this.electiveCourseAttendHour = 0.0;
            this.nonElectiveCourseMustAttendHourList = new Dictionary<CourseGroup, double>();
            this.electiveCourseMustAttendHour = 12.0;
        }
    }
}