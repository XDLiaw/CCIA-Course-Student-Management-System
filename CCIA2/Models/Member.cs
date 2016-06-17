namespace CCIA2.Models
{
    using System;
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
            MemberGroupApply = new HashSet<MemberGroupApply>();
            MemberGroupResult1 = new HashSet<MemberGroupResult>();
            MemberGroupApply1 = new HashSet<MemberGroupApply>();
        }

        [Key]
        public int sqno { get; set; }

        public int? mrJoinYear { get; set; }

        public int? mrMemberTypesqno { get; set; }

        [StringLength(50)]
        public string mrNumber { get; set; }

        [StringLength(50)]
        public string mrPassword { get; set; }

        [StringLength(50)]
        public string mrIsActive { get; set; }

        public DateTime? mrIsActivityDate { get; set; }

        [StringLength(50)]
        public string mrIsFinish { get; set; }

        public DateTime? mrFinishDate { get; set; }

        [StringLength(50)]
        public string mrName { get; set; }

        [StringLength(50)]
        public string mrId { get; set; }

        [StringLength(50)]
        public string mrBirth { get; set; }

        [StringLength(50)]
        public string mrTel { get; set; }

        [StringLength(50)]
        public string mrMobile { get; set; }

        public string mrMainEmail { get; set; }

        public string mrOtherEmail { get; set; }

        [StringLength(50)]
        public string mrAddress { get; set; }

        [StringLength(50)]
        public string mrNation { get; set; }

        [StringLength(50)]
        public string mrChineseLevel { get; set; }

        [StringLength(50)]
        public string mrEnglishLevel { get; set; }

        [StringLength(50)]
        public string mrJPLevel { get; set; }

        [StringLength(50)]
        public string mrTaiwanLevel { get; set; }

        [StringLength(50)]
        public string mrHakkaLevel { get; set; }

        [StringLength(50)]
        public string mrOtLanguageLevel { get; set; }

        [StringLength(50)]
        public string mrRole { get; set; }

        [StringLength(50)]
        public string mrSocialEdu { get; set; }

        [StringLength(50)]
        public string mrSocialComp { get; set; }

        [StringLength(50)]
        public string mrSocialTitle { get; set; }

        [StringLength(50)]
        public string mrCateType { get; set; }

        public int? mrCultureSqno { get; set; }

        [StringLength(50)]
        public string mrCultureOther { get; set; }

        [StringLength(50)]
        public string mrOtherCategory { get; set; }

        [StringLength(50)]
        public string mrStuSchool { get; set; }

        [StringLength(50)]
        public string mrStuDept { get; set; }

        [StringLength(50)]
        public string mrStuYear { get; set; }

        public string mrPro { get; set; }

        public string mrSkill { get; set; }

        public string mrWhy { get; set; }

        public string mrFuture { get; set; }

        public string mrOther { get; set; }

        [StringLength(50)]
        public string mrIsOpen { get; set; }

        public DateTime? mrCreateDt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberAttchFile> MemberAttchFile { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberGroupResult> MemberGroupResult { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberGroupApply> MemberGroupApply { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberGroupResult> MemberGroupResult1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MemberGroupApply> MemberGroupApply1 { get; set; }
    }
}
