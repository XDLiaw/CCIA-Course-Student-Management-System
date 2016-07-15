namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberCourseAttchFile")]
    public class MemberCourseAttchFile
    {
        [Key]
        public int sqno { get; set; }

        [ForeignKey("memberCourse")]
        public int mrCourseSqno { get; set; }

        public MemberCourse memberCourse { get; set; }

        public int mrSqno { get; set; }

        [StringLength(50)]
        public string mrNumber { get; set; }

        [Display(Name="ªþÀÉ")]
        [StringLength(500)]
        public string ShowAttchFileName { get; set; }

        [StringLength(500)]
        public string AttchFileName { get; set; }

        [StringLength(10)]
        public string AttchFileType { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
