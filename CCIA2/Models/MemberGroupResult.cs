namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberGroupResult")]
    public partial class MemberGroupResult
    {
        [Key]
        public int sqno { get; set; }

        public int? mrSqno { get; set; }

        [StringLength(50)]
        public string mrNumber { get; set; }

        public int AppraiseStep { get; set; }

        [StringLength(1)]
        public string AppraiseResult { get; set; }

        [StringLength(50)]
        public string AppraiseGroup { get; set; }

        public string AppraiseDesc { get; set; }

        public DateTime? AppraiseCreateDt { get; set; }

        [StringLength(50)]
        public string AppraiseNo { get; set; }

        public virtual Member Member { get; set; }

        public virtual Member Member1 { get; set; }
    }
}
