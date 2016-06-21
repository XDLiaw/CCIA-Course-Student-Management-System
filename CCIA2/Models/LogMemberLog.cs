namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LogMemberLog")]
    public partial class LogMemberLog
    {
        [Key]
        public int sqno { get; set; }

        [StringLength(50)]
        public string sessionid { get; set; }

        public int? mrsqno { get; set; }

        [StringLength(50)]
        public string mrIP { get; set; }

        public string mrPages { get; set; }

        public string mrQuery { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
