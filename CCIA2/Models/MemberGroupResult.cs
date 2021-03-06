namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("MemberGroupResult")]
    public partial class MemberGroupResult
    {
        [Key]
        public int sqno { get; set; }

        [ForeignKey("member")]
        public int mrSqno { get; set; }

        [StringLength(50)]
        public string mrNumber { get; set; }

        public virtual Member member { get; set; }

        [Display(Name="階段")]
        public int AppraiseStep { get; set; }

        [Display(Name="結果")]
        [StringLength(1)]
        public string AppraiseResult { get; set; }

        [Display(Name="審查員選組別")]
        [StringLength(50)]
        public string AppraiseGroup { get; set; }

        [Display(Name = "狀態")]
        [StringLength(50)]
        public string AppraiseState { get; set; }

        [Display(Name="分數")]
        [DisplayFormat(DataFormatString = "{0:F1}")]
        public double? AppraiseScore { get; set; }

        [Display(Name="審查員意見")]
        public string AppraiseDesc { get; set; }

        [Display(Name="審查日期")]
        public DateTime? AppraiseCreateDt { get; set; }

        [Display(Name="審查員編號")]
        [StringLength(50)]
        public string AppraiseNo { get; set; }

        public MemberGroupResult() { }

        public MemberGroupResult(Member member)
        {
            this.mrSqno = member.sqno;
            this.mrNumber = member.mrNumber;
            this.member = member;
            this.AppraiseCreateDt = DateTime.Now;
            SysUser user = HttpContext.Current.Session[SessionKey.USER] as SysUser;
            this.AppraiseNo = user.accountNo;
            member.MemberGroupResult.Add(this);
        }

    }
}
