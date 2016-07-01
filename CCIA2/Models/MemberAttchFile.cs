namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberAttchFile")]
    public partial class MemberAttchFile
    {
        [Key]
        public int sqno { get; set; }

        [ForeignKey("member")]
        public int mrSqno { get; set; }

        [StringLength(50)]
        public string mrNumber { get; set; }

        public virtual Member member { get; set; }

        public string mrShowFileName { get; set; }

        public string mrAttchFileName { get; set; }

        public string mrAttchFileType { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}
