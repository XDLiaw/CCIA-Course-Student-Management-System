using MvcPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCIA2.Models.ViewModels
{
    public class CourseRelativeViewModel
    {
        public string tabId { get; set; }

        public CourseGroupViewModel courseGroupViewModel { get; set; }

        public CourseViewModel courseViewModel { get; set; }

        public TeacherViewModel teacherViewModel { get; set; }

        public StudendViewModel studentViewModel { get; set; }

        public CourseRelativeViewModel()
        {
            this.courseGroupViewModel = new CourseGroupViewModel();
            this.courseViewModel = new CourseViewModel();
            this.teacherViewModel = new TeacherViewModel();
            this.studentViewModel = new StudendViewModel();
        }
    }

    public class CourseGroupViewModel
    {
        public int groupSqno { get; set; }

        public List<CourseGroup> courseGroupList { get; set; }
    }

    public class CourseViewModel
    {
        public int courseClassSqno { get; set; }

        public DateTime? day { get; set; }

        public string searchText { get; set; }

        [Display(Name = "頁碼")]
        public int pageNumber { get; set; }

        [Display(Name = "每頁資料筆數")]
        public int pageSize { get; private set; }

        public IPagedList<Course> CoursePagedList { get; set; }

        public CourseViewModel()
        {
            this.pageNumber = 1;
            this.pageSize = 15;
        }
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

    public class StudendViewModel
    {
        public string searchText { get; set; }

        [Display(Name="會員腳色")]
        public int? memberTypeSqno { get; set; }

        [Display(Name = "頁碼")]
        public int pageNumber { get; set; }

        [Display(Name = "每頁資料筆數")]
        public int pageSize { get; private set; }

        public IPagedList<Member> studentPagedList { get; set; }

        public StudendViewModel()
        {
            this.pageNumber = 1;
            this.pageSize = 15;
        }
    }
}