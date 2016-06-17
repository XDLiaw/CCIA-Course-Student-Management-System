namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TableNumber")]
    public partial class TableNumber
    {
        [Key]
        public int sqno { get; set; }

        [StringLength(50)]
        public string MemberNo { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
