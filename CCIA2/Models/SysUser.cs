namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SysUser")]
    public partial class SysUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sqno { get; set; }

        [Required]
        [StringLength(50)]
        public string accountNo { get; set; }

        [Required]
        [StringLength(50)]
        public string password { get; set; }

        public int role { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        public DateTime? createDate { get; set; }
    }
}
