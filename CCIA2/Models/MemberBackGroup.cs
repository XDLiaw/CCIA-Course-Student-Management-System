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

        public int? mrSqno { get; set; }

        [StringLength(50)]
        public string mrMemberNo { get; set; }

        [StringLength(50)]
        public string BackGroupYear { get; set; }

        [StringLength(50)]
        public string BacmGroupSqno { get; set; }
    }
}
