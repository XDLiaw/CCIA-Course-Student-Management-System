namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CourseDay")]
    public partial class CourseDay
    {
        [Key]
        public int sqno { get; set; }

        public int? CourseSqno { get; set; }

        [Column("CourseDay")]
        [StringLength(50)]
        public string CourseDay1 { get; set; }
    }
}
