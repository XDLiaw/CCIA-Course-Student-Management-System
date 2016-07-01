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

        [Display(Name="���q")]
        public int AppraiseStep { get; set; }

        [Display(Name="���G")]
        [StringLength(1)]
        public string AppraiseResult { get; set; }

        [Display(Name="�f�d����էO")]
        [StringLength(50)]
        public string AppraiseGroup { get; set; }

        [Display(Name = "���A")]
        [StringLength(50)]
        public string AppraiseState { get; set; }

        [Display(Name="����")]
        [DisplayFormat(DataFormatString = "{0:F1}")]
        public double? AppraiseScore { get; set; }

        [Display(Name="�f�d���N��")]
        public string AppraiseDesc { get; set; }

        [Display(Name="�f�d���")]
        public DateTime? AppraiseCreateDt { get; set; }

        [Display(Name="�f�d���s��")]
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
