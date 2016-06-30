namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TableApplyStep")]
    public partial class TableApplyStep
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int sqno { get; set; }

        public int applystep { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        public bool role1Auth { get; set; }

        public bool role2Auth { get; set; }

        public bool role3Auth { get; set; }
    }
}
