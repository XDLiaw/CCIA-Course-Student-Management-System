namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TableCulture")]
    public partial class TableCulture
    {
        [Key]
        public int sqno { get; set; }

        [StringLength(50)]
        public string CultureName { get; set; }
    }
}
