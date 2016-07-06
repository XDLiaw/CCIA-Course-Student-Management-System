using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCIA2.Models
{
    [Table("CourseTeacherRelation")]
    public class CourseTeacherRelation
    {
        [Key, Column(Order = 1)]
        [Required]
        [ForeignKey("course")]
        public int courseSqno { get; set; }

        public virtual Course course { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        [ForeignKey("teacher")]
        public int teacherSqno { get; set; }

        public virtual CourseTeacher teacher { get; set; }
    }
}