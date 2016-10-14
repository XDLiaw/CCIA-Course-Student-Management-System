using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCIA2.Models
{
    [Table("ActivityJoinPages")]
    public class ActivitySignUp
    {
        [Key]
        public int sqno { get; set; }

        [ForeignKey("activity")]
        public int activitysqno { get; set; }

        [Display(Name = "活動")]
        public Activity activity { get; set; }

        [Display(Name = "姓名")]
        [StringLength(500)]
        [Required]
        public string name { get; set; }

        [Display(Name = "手機")]
        [StringLength(500)]
        [Required]
        public string mobile { get; set; }

        [Display(Name = "電話")]
        [StringLength(500)]
        public string phone { get; set; }

        [Display(Name = "Email(1)")]
        [Column("EmailOne")]
        [StringLength(5000)]
        public string email1 { get; set; }

        [Display(Name = "Email(2)")]
        [Column("EmailTwo")]
        [StringLength(5000)]
        public string email2 { get; set; }

        [Display(Name = "身份")]
        [StringLength(50)]
        [Required]
        public string role { get; set; }

        [Display(Name = "身份")]
        public string roleName
        {
            get
            {
                if (this.role == "1") return "社會人士";
                if (this.role == "2") return "學生";
                return "其他";
            }
        }

        //------------社會人士-----------------------------------

        [Display(Name = "最高學歷")]
        [StringLength(50)]
        public string socEdu { get; set; }

        [Display(Name = "服務機關")]
        [StringLength(5000)]
        public string socComp { get; set; }

        [Display(Name = "職稱")]
        [StringLength(5000)]
        public string socCompTitle { get; set; }

        [Display(Name = "產業別")]
        [StringLength(5000)]
        public string socCompType { get; set; }

        [Display(Name = "產業")]
        [ForeignKey("socCulture")]
        public int? socCultureSqno { get; set; }

        [Display(Name = "產業")]
        public virtual TableCulture socCulture { get; set; }

        [Display(Name = "其他產業別")]
        [StringLength(5000)]
        public string socCompOtherCagegory { get; set; }

        [Display(Name = "其他產業")]
        [StringLength(5000)]
        public string socCompOther { get; set; }

        //------------學生-----------------------------------

        [Display(Name = "學校名稱")]
        [StringLength(5000)]
        public string stuSchName { get; set; }

        [Display(Name = "科系")]
        [StringLength(5000)]
        public string stuDept { get; set; }

        [Display(Name = "年級")]
        [StringLength(5000)]
        public string stuYear { get; set; }

        //---------------------------------------------------

        [StringLength(2)]
        public string gender { get; set; }

        [Display(Name = "用餐")]
        [StringLength(5000)]
        public string meal { get; set; }

        [Display(Name = "是否出席")]
        [StringLength(1)]
        public string isCome { get; set; }

    }
}