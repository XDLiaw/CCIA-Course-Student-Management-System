namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberGroupResult")]
    public partial class MemberGroupResult
    {
        [Key]
        public int sqno { get; set; }

        public int? mrSqno { get; set; }

        [StringLength(50)]
        public string mrNumber { get; set; }

        [Display(Name="階段")]
        public int AppraiseStep { get; set; }

        [Display(Name="結果")]
        [StringLength(1)]
        public string AppraiseResult { get; set; }

        [Display(Name="審查員選組別")]
        [StringLength(50)]
        public string AppraiseGroup { get; set; }

        [Display(Name="審查員意見")]
        public string AppraiseDesc { get; set; }

        [Display(Name="審查日期")]
        public DateTime? AppraiseCreateDt { get; set; }

        [Display(Name="審查員編號")]
        [StringLength(50)]
        public string AppraiseNo { get; set; }

        public virtual Member Member { get; set; }

        public string currentStateString()
        {
            if (this.AppraiseStep == 1)
            {
                return "資格審";
            }
            else if (this.AppraiseStep == 2)
            {
                return "初審";
            }
            else if (this.AppraiseStep == 3)
            {
                return "錄取";
            }
            else if (this.AppraiseStep == 4)
            {
                return "繳交保證金";
            }
            return "";
        }

        public string appraiseResultString()
        {
            if (this.AppraiseStep == 1 || this.AppraiseStep == 2)
            {
                return Int32.Parse(this.AppraiseResult) > 0 ? "通過" : "未通過";
            }
            if (this.AppraiseStep == 3)
            {
                if (Int32.Parse(this.AppraiseResult) == 1) return "正取";
                else if (Int32.Parse(this.AppraiseResult) == 2) return "備取";
                else return "未錄取";
            }
            if (this.AppraiseStep == 4)
            {
                return Int32.Parse(this.AppraiseResult) > 0 ? "是" : "否";
            }
            return "";
        }
    }
}
