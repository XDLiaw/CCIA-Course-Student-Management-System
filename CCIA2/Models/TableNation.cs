namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TableNation")]
    public partial class TableNation
    {
        [Key]
        public int sqno { get; set; }

        [StringLength(50)]
        public string NationName { get; set; }
    }
}
