using MvcPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCIA2.Models.ViewModels
{
    public class CourseViewModel
    {
        public string tabId { get; set; }

        public CourseGroupViewModel courseGroupViewModel { get; set; }

        public TeacherViewModel teacherViewModel { get; set; }

        public CourseViewModel()
        {
            this.courseGroupViewModel = new CourseGroupViewModel();
            this.teacherViewModel = new TeacherViewModel();
        }
    }

    public class CourseGroupViewModel
    {
        public int groupSqno { get; set; }

        public List<CourseGroup> courseGroupList { get; set; }
    }

    public class TeacherViewModel
    {
        public string searchText { get; set; }

        [Display(Name = "頁碼")]
        public int pageNumber { get; set; }

        [Display(Name = "每頁資料筆數")]
        public int pageSize { get; private set; }

        public IPagedList<CourseTeacher> teacherPagedList { get; set; }

        public TeacherViewModel()
        {
            this.pageNumber = 1;
            this.pageSize = 15;
        }
    }
}