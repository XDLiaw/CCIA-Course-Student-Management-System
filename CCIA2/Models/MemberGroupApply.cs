namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberGroupApply")]
    public partial class MemberGroupApply
    {
        [Key]
        public int sqno { get; set; }

        public int? mrSqno { get; set; }

        [StringLength(50)]
        public string mrNumber { get; set; }

        [StringLength(50)]
        public string First { get; set; }

        [StringLength(50)]
        public string Second { get; set; }

        [StringLength(50)]
        public string Third { get; set; }

        public DateTime? CreateDate { get; set; }

        public virtual Member Member { get; set; }

        public virtual Member Member1 { get; set; }
    }
}
