using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCIA2.Models
{
    [Table("ActivityPages")]
    public class Activity
    {
        [Key]
        public int sqno { get; set; }

        [Display(Name = "活動名稱")]
        [StringLength(500)]
        public string activitytitle { get; set; }

        [Display(Name="活動內容")]
        [AllowHtml]
        public string activityContent { get; set; }

        [Display(Name = "開始日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? startDate { get; set; }

        [Display(Name = "結束日期")]
        [Column("edDate")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? endDate { get; set; }

        [Display(Name = "截止日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? deadline { get; set; }

        [StringLength(1)]
        public string meal { get; set; }

        [Display(Name = "供餐")]
        [NotMapped]
        public bool isNeedMeal 
        {
            get
            {
                return this.meal == "Y";
            }
            set
            {
                this.meal = value ? "Y" : "N";
            }        
        }

        public DateTime? CreateDate { get; set; }
    }
}