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

        public int? mrSqno { get; set; }

        [StringLength(50)]
        public string mrNumber { get; set; }

        public int? PlanSqno { get; set; }

        [StringLength(50)]
        public string SupportName { get; set; }

        [StringLength(50)]
        public string SupportYear { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
