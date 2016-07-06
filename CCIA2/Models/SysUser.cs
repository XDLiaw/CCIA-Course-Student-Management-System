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

        [Display(Name="�b��")]
        [Required]
        [StringLength(50)]
        public string accountNo { get; set; }

        [Display(Name="�K�X")]
        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Display(Name="�}��")]
        public int role { get; set; }

        [Display(Name="�m�W")]
        [StringLength(50)]
        public string name { get; set; }

        [Display(Name="�~���H���էO")]
        [StringLength(50)]
        public string group { get; set; }

        public DateTime? createDate { get; set; }
    }
}
