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

        public TableGroup tableGroup { get; set; }

        [ForeignKey("courseClass")]
        public int courseClassSqno { get; set; }

        public CourseClass courseClass { get; set; }

        [Display(Name="是否為選修課")]
        public bool isElective { get; set; }
    }
}