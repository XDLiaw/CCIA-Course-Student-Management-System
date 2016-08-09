using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCIA2.Models
{
    [Table("CourseClass")]
    public class CourseClass
    {
        [Key]
        public int sqno { get; set; }

        [Display(Name="課程類別名稱")]
        [StringLength(50)]
        public string name { get; set; }

        public virtual ICollection<Course> courses { get; set; }
    }
}