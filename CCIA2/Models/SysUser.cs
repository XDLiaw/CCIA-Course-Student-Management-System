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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int sqno { get; set; }

        [Display(Name="帳號")]
        [Required]
        [StringLength(50)]
        public string accountNo { get; set; }

        [Display(Name="密碼")]
        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name="腳色")]
        public int role { get; set; }

        [Display(Name="姓名")]
        [StringLength(50)]
        public string name { get; set; }

        [Display(Name="外評人員組別")]
        [StringLength(50)]
        public string group { get; set; }

        public DateTime? createDate { get; set; }
    }
}
