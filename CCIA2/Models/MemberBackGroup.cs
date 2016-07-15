namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberBackGroup")]
    public partial class MemberBackGroup
    {
        [Key]
        public int sqno { get; set; }

        [ForeignKey("member")]
        public int mrSqno { get; set; }

        [StringLength(50)]
        public string mrMemberNo { get; set; }

        public virtual Member member { get; set; }

        [Display(Name="¨ü°V¦~«×")]
        [StringLength(50)]
        public string BackGroupYear { get; set; }

        [ForeignKey("tableBackGroup")]
        public int BackGroupSqno { get; set; }

        public virtual TableBackGroup tableBackGroup { get; set; }
    }
}
