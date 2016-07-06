using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcPaging;
using System.ComponentModel.DataAnnotations;

namespace CCIA2.Models.ViewModels
{

    public class CourseTeacherViewModel
    {
        public string searchText { get; set; }

        [Display(Name = "頁碼")]
        public int pageNumber { get; set; }

        [Display(Name = "每頁資料筆數")]
        public int pageSize { get; private set; }

        public IPagedList<CourseTeacher> teacherPagedList { get; set; }

        public CourseTeacherViewModel()
        {
            this.pageNumber = 1;
            this.pageSize = 15;
        }
    }
}