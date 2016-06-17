namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TableGroup")]
    public partial class TableGroup
    {
        [Key]
        public int sqno { get; set; }

        [StringLength(50)]
        public string GroupNo { get; set; }

        [StringLength(50)]
        public string GroupName { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
