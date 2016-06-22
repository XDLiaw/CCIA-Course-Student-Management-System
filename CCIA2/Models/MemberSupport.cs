namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberSupport")]
    public partial class MemberSupport
    {
        [Key]
        public int sqno { get; set; }

        [ForeignKey("member")]
        public int? mrSqno { get; set; }

        public virtual Member member { get; set; }

        [StringLength(50)]
        public string mrNumber { get; set; }

        [ForeignKey("plan")]
        public int? PlanSqno { get; set; }

        public virtual TablePlan plan { get; set; }

        [Display(Name = "計畫名稱")]
        [StringLength(50)]
        public string SupportName { get; set; }

        [Display(Name = "年度")]
        [StringLength(50)]
        public string SupportYear { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
