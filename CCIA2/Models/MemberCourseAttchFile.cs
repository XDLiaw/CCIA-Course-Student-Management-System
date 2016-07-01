namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberCourseAttchFile")]
    public partial class MemberCourseAttchFile
    {
        [Key]
        public int sqno { get; set; }

        public int? CourseSqno { get; set; }

        public int mrSqno { get; set; }

        [StringLength(50)]
        public string mrNumber { get; set; }

        [StringLength(50)]
        public string MemberAttchFileName { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
