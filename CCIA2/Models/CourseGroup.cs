using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCIA2.Models
{
    [Table("CourseGroup")]
    public class CourseGroup
    {
        [Key]
        public int sqno { get; set; }

        [ForeignKey("tableGroup")]
        public int memberGroupSqno { get; set; }

        [Display(Name = "組別")]
        public virtual TableGroup tableGroup { get; set; }

        [ForeignKey("courseClass")]
        public int courseClassSqno { get; set; }

        [Display(Name = "課程大綱")]
        public virtual CourseClass courseClass { get; set; }

        [Display(Name = "是否為選修課")]
        public bool isElective { get; set; }

        [Display(Name = "必修/選修")]
        [NotMapped]
        public string electiveString
        {
            get
            {
                return isElective ? "選修" : "必修";
            }
        }
    }
}