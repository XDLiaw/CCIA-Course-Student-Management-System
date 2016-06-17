namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TableMemberType")]
    public partial class TableMemberType
    {
        [Key]
        public int sqno { get; set; }

        public int? membertypeno { get; set; }

        [StringLength(50)]
        public string membertypename { get; set; }
    }
}
