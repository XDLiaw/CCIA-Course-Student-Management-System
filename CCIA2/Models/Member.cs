namespace CCIA2.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;
    using CCIA2.Models.ViewModels;

    [Table("Member")]
    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            MemberAttchFile = new List<MemberAttchFile>();
            MemberGroupResult = new List<MemberGroupResult>();
            MemberGroupApply = new List<MemberGroupApply>();
            MemberSupport = new List<MemberSupport>();
            memberBackGroups = new List<MemberBackGroup>();
        }

        [Key]
        public int sqno { get; set; }

        [Display(Name = "受訓年度")]
        public int? mrJoinYear { get; set; }

        [Display(Name = "會員角色")]
        [ForeignKey("memberType")]
        public int? mrMemberTypesqno { get; set; }

        [Display(Name = "會員角色")]
        public virtual TableMemberType memberType { get; set; }

        [Display(Name = "會員編號")]
        [StringLength(50)]
        public string mrNumber { get; set; }

        [Display(Name="密碼")]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string mrPassword { get; set; }

        [Display(Name = "啟動")]
        [StringLength(50)]
        public string mrIsActive { get; set; }

        [Display(Name = "啟動日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? mrIsActivityDate { get; set; }

        [Display(Name = "完成")]
        [StringLength(50)]
        public string mrIsFinish { get; set; }

        [Display(Name = "完成日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? mrFinishDate { get; set; }

        [Display(Name = "姓名")]
        [StringLength(50)]
        public string mrName { get; set; }

        [Display(Name = "身份證字號")]
        [StringLength(50)]
        public string mrId { get; set; }

        [Display(Name = "生日")]
        [StringLength(50)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public string mrBirth { get; set; }

        [Display(Name = "電話")]
        [StringLength(50)]
        public string mrTel { get; set; }

        [Display(Name = "手機")]
        [StringLength(50)]
        public string mrMobile { get; set; }

        [Display(Name = "EMail(1)")]
        public string mrMainEmail { get; set; }

        [Display(Name = "EMail(2)")]
        public string mrOtherEmail { get; set; }

        [Display(Name = "聯絡地址")]
        [StringLength(50)]
        public string mrAddress { get; set; }

        [Display(Name = "是否為原住民或客家")]
        [StringLength(50)]
        public string mrNation { get; set; }

        [Display(Name = "中文能力")]
        [StringLength(50)]
        public string mrChineseLevel { get; set; }

        [Display(Name = "英文能力")]
        [StringLength(50)]
        public string mrEnglishLevel { get; set; }

        [Display(Name = "日文能力")]
        [StringLength(50)]
        public string mrJPLevel { get; set; }

        [Display(Name = "台語能力")]
        [StringLength(50)]
        public string mrTaiwanLevel { get; set; }

        [Display(Name = "客家話能力")]
        [StringLength(50)]
        public string mrHakkaLevel { get; set; }

        [Display(Name = "其他語言能力")]
        [StringLength(50)]
        public string mrOtLanguageLevel { get; set; }

        [Display(Name = "身份")]
        [StringLength(50)]
        public string mrRole { get; set; }

        [Display(Name = "最高學歷")]
        [StringLength(50)]
        public string mrSocialEdu { get; set; }

        [Display(Name = "服務機關")]
        [StringLength(50)]
        public string mrSocialComp { get; set; }

        [Display(Name = "職稱")]
        [StringLength(50)]
        public string mrSocialTitle { get; set; }

        [Display(Name = "產業別")]
        [StringLength(50)]
        public string mrCateType { get; set; }

        [Display(Name = "產業")]
        [ForeignKey("culture")]
        public int? mrCultureSqno { get; set; }

        [Display(Name = "產業")]
        public virtual TableCulture culture { get; set; }

        [Display(Name = "其它經中央主管機關指定之產業")]
        [StringLength(50)]
        public string mrCultureOther { get; set; }

        [Display(Name = "其他產業別")]
        [StringLength(50)]
        public string mrOtherCategory { get; set; }

        [Display(Name = "就讀學校")]
        [StringLength(50)]
        public string mrStuSchool { get; set; }

        [Display(Name = "就讀系所")]
        [StringLength(50)]
        public string mrStuDept { get; set; }

        [Display(Name = "年級")]
        [StringLength(50)]
        public string mrStuYear { get; set; }

        [Display(Name = "專長")]
        public string mrPro { get; set; }

        [Display(Name = "重要經歷")]
        public string mrSkill { get; set; }

        [Display(Name = "報名本屆課程參與目的")]
        public string mrWhy { get; set; }

        [Display(Name = "對經紀人未來規劃與構想")]
        public string mrFuture { get; set; }

        [Display(Name = "其他補充說明")]
        public string mrOther { get; set; }

        [Display(Name = "是否願意公開資料")]
        [StringLength(50)]
        public string mrIsOpen { get; set; }

        [Display(Name = "願意公開Email")]
        [StringLength(50)]
        public string isOpenEmail { get; set; }

        [Display(Name = "願意公開手機")]
        [StringLength(50)]
        public string isOpenMobile { get; set; }

        [Display(Name = "願意公開電話")]
        [StringLength(50)]
        public string isOpenPhone { get; set; }

        [Display(Name = "同意透過系統服務信箱發送訊息至個人信箱，以便有需要之相關業者留言詢問")]
        [StringLength(50)]
        public string isAgreeSendMailtoU { get; set; }

        [Display(Name = "")]
        public DateTime? mrCreateDt { get; set; }

        [Display(Name = "上傳相關文件")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberAttchFile> MemberAttchFile { get; set; }

        [Display(Name = "個階段結果")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberGroupResult> MemberGroupResult { get; set; }

        [Display(Name = "初審平均分數")]
        [DisplayFormat(DataFormatString = "{0:F1}")]
        [NotMapped]
        public double? firstTrailScore
        {
            get
            {
                try
                {
                    return this.MemberGroupResult.Where(res => res.AppraiseStep == 2).Average(res => res.AppraiseScore);
                }
                catch (NullReferenceException e)
                {
                    return null;
                }
            }
        }

        [Display(Name="初審評分人數")]
        public int numberOfFristTrailScore
        {
            get
            {
                return MemberGroupResult.Count(res => res.AppraiseStep == 2);
            }
        }

        [Display(Name = "複審平均分數")]
        [DisplayFormat(DataFormatString = "{0:F1}")]
        [NotMapped]
        public double? secondTrailAvgScore
        {
            get
            {
                try
                {
                    return this.MemberGroupResult.Where(res => res.AppraiseStep == 4).Average(res => res.AppraiseScore);
                }
                catch (NullReferenceException e)
                {
                    return null;
                }
            }
        }

        [Display(Name = "複審評分人數")]
        public int numberOfSecondTrailScore
        {
            get
            {
                return MemberGroupResult.Count(res => res.AppraiseStep == 4);
            }
        }

        [Display(Name = "報名組別")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberGroupApply> MemberGroupApply { get; set; }

        [Display(Name = "資格審組別")]
        [NotMapped]
        public string FirstAssignGroup
        {
            get
            {
                var temp = this.MemberGroupResult.Where(res => res.AppraiseStep == 1).FirstOrDefault();
                return temp == null ? null : temp.AppraiseGroup;
            }
        }

        [Display(Name = "最終組別")]
        [NotMapped]
        public string FinalGroup
        {
            get
            {
                var temp = this.MemberGroupResult.Where(res => res.AppraiseStep == 5).FirstOrDefault();
                return temp == null ? null : temp.AppraiseGroup;
            }
        }

        [Display(Name = "曾經參加或獲得的文化部計畫補助")]
        public virtual ICollection<MemberSupport> MemberSupport { get; set; }

        
        public virtual ICollection<MemberBackGroup> memberBackGroups { get; set; }

        [Display(Name = "受訓年度/組別")]
        [NotMapped]
        public List<HistoryYearGroupViewModel> memberHistoryGroups
        {
            get
            {
                List<HistoryYearGroupViewModel> res = new List<HistoryYearGroupViewModel>();
                if (this.memberBackGroups != null && this.memberBackGroups.Count > 0)
                {
                    this.memberBackGroups.ToList().ForEach(x => res.Add(new HistoryYearGroupViewModel() { year = x.BackGroupYear, groupName = x.tableBackGroup.backGroupName }));
                }                
                if (this.FinalGroup != null)
                {
                    res.Add(new HistoryYearGroupViewModel() { year = this.mrJoinYear.ToString(), groupName = this.FinalGroup });
                }
                return res;
            }
        }

        [Display(Name="狀態")]
        [NotMapped]
        public string currentState
        {
            get
            {
                SysUser user = HttpContext.Current.Session[SessionKey.USER] as SysUser;
                if (this.MemberGroupResult == null || this.MemberGroupResult.Count == 0)
                {
                    return "待審核";
                }
                else
                {
                    if (user.role == 1 || user.role == 2)
                    {
                        if (this.MemberGroupResult.Count(res => res.AppraiseStep == 1) == 1 && // 已有"通過資格審"資料一筆
                            this.MemberGroupResult.Count(res => res.AppraiseStep > 1 && res.AppraiseNo == user.accountNo) == 0 && // 表自己還沒評初審過
                            this.MemberGroupResult.Count(res => res.AppraiseStep >= 3) == 0)  // 不能有正備取紀錄或未通過紀錄或進入複審紀錄
                        {
                            return this.MemberGroupResult.Where(res => res.AppraiseStep == 1).LastOrDefault().AppraiseState;
                        }

                        if (this.MemberGroupResult.Count(res => res.AppraiseStep == 3) == 1 && // 已有一筆"進行複審"的資料 (已在複審待審核清單中)
                            this.MemberGroupResult.Count(res => res.AppraiseStep > 3 && res.AppraiseNo == user.accountNo) == 0 && // 自己還沒評過複審
                            this.MemberGroupResult.Count(res => res.AppraiseStep >= 5) == 0) // 不能有正備取or通過or未通過紀錄
                        {
                            return this.MemberGroupResult.Where(res => res.AppraiseStep == 3).LastOrDefault().AppraiseState;
                        }
                    }
                    return this.MemberGroupResult.OrderBy(m => m.AppraiseStep).LastOrDefault().AppraiseState;
                }
            }
        }

    }
}
