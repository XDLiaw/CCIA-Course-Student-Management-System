namespace CCIA2.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Member")]
    public partial class Member
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Member()
        {
            MemberAttchFile = new HashSet<MemberAttchFile>();
            MemberGroupResult = new HashSet<MemberGroupResult>();
            //MemberGroupApply = new HashSet<MemberGroupApply>();
        }

        [Key]
        public int sqno { get; set; }

        [Display(Name = "���V�~��")]
        public int? mrJoinYear { get; set; }

        [Display(Name = "�|������")]
        [ForeignKey("memberType")]
        public int? mrMemberTypesqno { get; set; }

        [Display(Name = "�|������")]
        public virtual TableMemberType memberType { get; set; }

        [Display(Name = "�|���s��")]
        [StringLength(50)]
        public string mrNumber { get; set; }

        [Display(Name="�K�X")]
        [StringLength(50)]
        public string mrPassword { get; set; }

        [Display(Name = "�Ұ�")]
        [StringLength(50)]
        public string mrIsActive { get; set; }

        [Display(Name = "�Ұʤ��")]
        public DateTime? mrIsActivityDate { get; set; }

        [Display(Name = "����")]
        [StringLength(50)]
        public string mrIsFinish { get; set; }

        [Display(Name = "�������")]
        public DateTime? mrFinishDate { get; set; }

        [Display(Name = "�m�W")]
        [StringLength(50)]
        public string mrName { get; set; }

        [Display(Name = "�����Ҧr��")]
        [StringLength(50)]
        public string mrId { get; set; }

        [Display(Name = "�ͤ�")]
        [StringLength(50)]
        public string mrBirth { get; set; }

        [Display(Name = "�q��")]
        [StringLength(50)]
        public string mrTel { get; set; }

        [Display(Name = "���")]
        [StringLength(50)]
        public string mrMobile { get; set; }

        [Display(Name = "EMail(1)")]
        public string mrMainEmail { get; set; }

        [Display(Name = "EMail(2)")]
        public string mrOtherEmail { get; set; }

        [Display(Name = "�p���a�}")]
        [StringLength(50)]
        public string mrAddress { get; set; }

        [Display(Name = "�O�_�������ΫȮa")]
        [StringLength(50)]
        public string mrNation { get; set; }

        [Display(Name = "�����O")]
        [StringLength(50)]
        public string mrChineseLevel { get; set; }

        [Display(Name = "�^���O")]
        [StringLength(50)]
        public string mrEnglishLevel { get; set; }

        [Display(Name = "����O")]
        [StringLength(50)]
        public string mrJPLevel { get; set; }

        [Display(Name = "�x�y��O")]
        [StringLength(50)]
        public string mrTaiwanLevel { get; set; }

        [Display(Name = "�Ȯa�ܯ�O")]
        [StringLength(50)]
        public string mrHakkaLevel { get; set; }

        [Display(Name = "��L�y����O")]
        [StringLength(50)]
        public string mrOtLanguageLevel { get; set; }

        [Display(Name = "����")]
        [StringLength(50)]
        public string mrRole { get; set; }

        [Display(Name = "�̰��Ǿ�")]
        [StringLength(50)]
        public string mrSocialEdu { get; set; }

        [Display(Name = "�A�Ⱦ���")]
        [StringLength(50)]
        public string mrSocialComp { get; set; }

        [Display(Name = "¾��")]
        [StringLength(50)]
        public string mrSocialTitle { get; set; }

        [Display(Name = "���~�O")]
        [StringLength(50)]
        public string mrCateType { get; set; }

        [Display(Name = "���~")]
        [ForeignKey("culture")]
        public int? mrCultureSqno { get; set; }

        [Display(Name = "���~")]
        public virtual TableCulture culture { get; set; }

        [Display(Name = "�䥦�g�����D�޾������w�����~")]
        [StringLength(50)]
        public string mrCultureOther { get; set; }

        [Display(Name = "��L���~�O")]
        [StringLength(50)]
        public string mrOtherCategory { get; set; }

        [Display(Name = "�NŪ�Ǯ�")]
        [StringLength(50)]
        public string mrStuSchool { get; set; }

        [Display(Name = "�NŪ�t��")]
        [StringLength(50)]
        public string mrStuDept { get; set; }

        [Display(Name = "�~��")]
        [StringLength(50)]
        public string mrStuYear { get; set; }

        [Display(Name = "�M��")]
        public string mrPro { get; set; }

        [Display(Name = "���n�g��")]
        public string mrSkill { get; set; }

        [Display(Name = "���W�����ҵ{�ѻP�ت�")]
        public string mrWhy { get; set; }

        [Display(Name = "��g���H���ӳW���P�c�Q")]
        public string mrFuture { get; set; }

        [Display(Name = "��L�ɥR����")]
        public string mrOther { get; set; }

        [Display(Name = "�O�_�@�N���}���")]
        [StringLength(50)]
        public string mrIsOpen { get; set; }

        [Display(Name = "")]
        public DateTime? mrCreateDt { get; set; }

        [Display(Name = "�W�Ǭ������")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberAttchFile> MemberAttchFile { get; set; }

        [Display(Name = "���A")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberGroupResult> MemberGroupResult { get; set; }

        [Display(Name = "���W�էO")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberGroupApply> MemberGroupApply { get; set; }

        public string currentStateString()
        {
            
            if (this.MemberGroupResult == null || this.MemberGroupResult.Count == 0)
            {
                return "�ݼf��";
            }
            else
            {
                MemberGroupResult currentRes = this.MemberGroupResult.OrderBy(r => r.AppraiseStep).ToList().LastOrDefault();
                if (currentRes.AppraiseStep == 1 && Int32.Parse(currentRes.AppraiseResult) == 0)
                {
                    return "���q�L���f";
                }
                else if (currentRes.AppraiseStep == 1 && Int32.Parse(currentRes.AppraiseResult) == 1)
                {
                    return "�w�q�L���f";
                }
                else if (currentRes.AppraiseStep == 2 && Int32.Parse(currentRes.AppraiseResult) == 0)
                {
                    return "���q�L��f";
                }
                else if (currentRes.AppraiseStep == 2 && Int32.Parse(currentRes.AppraiseResult) == 1)
                {
                    return "�w�q�L��f";
                }
                else if (currentRes.AppraiseStep == 3 && Int32.Parse(currentRes.AppraiseResult) == 0)
                {
                    return "������";
                }
                else if (currentRes.AppraiseStep == 3 && Int32.Parse(currentRes.AppraiseResult) == 1)
                {
                    return "����";
                }
                else if (currentRes.AppraiseStep == 3 && Int32.Parse(currentRes.AppraiseResult) == 2)
                {
                    return "�ƨ�";
                }
                else if (currentRes.AppraiseStep == 4 && Int32.Parse(currentRes.AppraiseResult) == 0)
                {
                    return "��ú�O�Ҫ�";
                }
                else if (currentRes.AppraiseStep == 4 && Int32.Parse(currentRes.AppraiseResult) == 1)
                {
                    return "�wú�O�Ҫ�";
                }
            }
            return "";
        }
    }
}
