using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCIA2.Models
{
    [Table("CourseTeacher")]
    public class CourseTeacher
    {
        public CourseTeacher()
        {
            this.courses = new HashSet<CourseTeacherRelation>();
        }

        [Key]
        public int sqno { get; set; }

        //[ForeignKey("course")]
        //public int courseSqno { get; set; }

        //public virtual Course course { get; set; }

        public virtual ICollection<CourseTeacherRelation> courses { get; set; }

        [Display(Name="講師姓名")]
        [StringLength(50)]
        public string name { get; set; }

        [Display(Name = "所屬單位")]
        [StringLength(50)]
        public string orgName { get; set; }

        [Display(Name = "講師簡介")]
        [AllowHtml]
        public string introduction { get; set; }
    }
}