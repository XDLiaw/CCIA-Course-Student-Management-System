namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SYS_ErrorLog
    {
        [Key]
        public int sqno { get; set; }

        public DateTime? CreateDate { get; set; }

        [StringLength(8)]
        public string CreateBy { get; set; }

        [StringLength(1)]
        public string LoginType { get; set; }

        [StringLength(500)]
        public string URL { get; set; }

        [StringLength(200)]
        public string QString { get; set; }

        [StringLength(80)]
        public string IP { get; set; }

        [StringLength(50)]
        public string code { get; set; }

        public int? num { get; set; }

        [StringLength(200)]
        public string src { get; set; }

        [StringLength(50)]
        public string cat { get; set; }

        [StringLength(500)]
        public string xfile { get; set; }

        public int? line { get; set; }

        public int? xcolumn { get; set; }

        [StringLength(5000)]
        public string xdesc { get; set; }

        [StringLength(500)]
        public string adesc { get; set; }

        [StringLength(8000)]
        public string form_data { get; set; }

        [StringLength(10)]
        public string form_method { get; set; }

        [StringLength(8000)]
        public string session_data { get; set; }
    }
}
