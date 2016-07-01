namespace CCIA2.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CCIAContext : DbContext
    {
        public CCIAContext()
            : base("name=CCIAContext")
        {
        }

        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<CourseDay> CourseDay { get; set; }
        public virtual DbSet<LogMemberLog> LogMemberLog { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<MemberAttchFile> MemberAttchFile { get; set; }
        public virtual DbSet<MemberBackGroup> MemberBackGroup { get; set; }
        public virtual DbSet<MemberCourseAttchFile> MemberCourseAttchFile { get; set; }
        public virtual DbSet<MemberGroupApply> MemberGroupApply { get; set; }
        public virtual DbSet<MemberGroupResult> MemberGroupResult { get; set; }
        public virtual DbSet<MemberSupport> MemberSupport { get; set; }
        public virtual DbSet<SYS_ErrorLog> SYS_ErrorLog { get; set; }
        public virtual DbSet<SysUser> SysUser { get; set; }
        public virtual DbSet<TableApplyStep> TableApplyStep { get; set; }
        public virtual DbSet<TableBackGroup> TableBackGroup { get; set; }
        public virtual DbSet<TableCulture> TableCulture { get; set; }
        public virtual DbSet<TableGroup> TableGroup { get; set; }
        public virtual DbSet<TableMemberType> TableMemberType { get; set; }
        public virtual DbSet<TableNation> TableNation { get; set; }
        public virtual DbSet<TableNumber> TableNumber { get; set; }
        public virtual DbSet<TablePlan> TablePlan { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseDay>()
                .Property(e => e.CourseDay1)
                .IsUnicode(false);

            modelBuilder.Entity<LogMemberLog>()
                .Property(e => e.sessionid)
                .IsUnicode(false);

            modelBuilder.Entity<LogMemberLog>()
                .Property(e => e.mrIP)
                .IsUnicode(false);

            modelBuilder.Entity<LogMemberLog>()
                .Property(e => e.mrPages)
                .IsUnicode(false);

            modelBuilder.Entity<LogMemberLog>()
                .Property(e => e.mrQuery)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrIsActive)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrIsFinish)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrId)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrBirth)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrTel)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrMobile)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrMainEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrOtherEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrNation)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrChineseLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrEnglishLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrJPLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrTaiwanLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrHakkaLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrOtLanguageLevel)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrRole)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrSocialEdu)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrSocialComp)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrSocialTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrCateType)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrCultureOther)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrOtherCategory)
                .IsUnicode(false);

            modelBuilder.Entity<Member>()
                .Property(e => e.mrIsOpen)
                .IsUnicode(false);

            //modelBuilder.Entity<Member>()
            //    .HasMany(e => e.MemberAttchFile)
            //    .WithOptional(e => e.Member)
            //    .HasForeignKey(e => e.mrSqno);

            //modelBuilder.Entity<Member>()
            //    .HasMany(e => e.MemberGroupResult)
            //    .WithOptional(e => e.Member)
            //    .HasForeignKey(e => e.mrSqno);

            modelBuilder.Entity<MemberAttchFile>()
                .Property(e => e.mrNumber)
                .IsUnicode(false);

            modelBuilder.Entity<MemberAttchFile>()
                .Property(e => e.mrShowFileName)
                .IsUnicode(false);

            modelBuilder.Entity<MemberAttchFile>()
                .Property(e => e.mrAttchFileName)
                .IsUnicode(false);

            modelBuilder.Entity<MemberAttchFile>()
                .Property(e => e.mrAttchFileType)
                .IsUnicode(false);

            modelBuilder.Entity<MemberBackGroup>()
                .Property(e => e.mrMemberNo)
                .IsUnicode(false);

            modelBuilder.Entity<MemberBackGroup>()
                .Property(e => e.BackGroupYear)
                .IsUnicode(false);

            modelBuilder.Entity<MemberBackGroup>()
                .Property(e => e.BacmGroupSqno)
                .IsUnicode(false);

            modelBuilder.Entity<MemberCourseAttchFile>()
                .Property(e => e.mrNumber)
                .IsUnicode(false);

            modelBuilder.Entity<MemberCourseAttchFile>()
                .Property(e => e.MemberAttchFileName)
                .IsUnicode(false);

            modelBuilder.Entity<MemberGroupApply>()
                .Property(e => e.mrNumber)
                .IsUnicode(false);

            modelBuilder.Entity<MemberGroupApply>()
                .Property(e => e.First)
                .IsUnicode(false);

            modelBuilder.Entity<MemberGroupApply>()
                .Property(e => e.Second)
                .IsUnicode(false);

            modelBuilder.Entity<MemberGroupApply>()
                .Property(e => e.Third)
                .IsUnicode(false);

            modelBuilder.Entity<MemberGroupResult>()
                .Property(e => e.mrNumber)
                .IsUnicode(false);

            modelBuilder.Entity<MemberGroupResult>()
                .Property(e => e.AppraiseResult)
                .IsUnicode(false);

            modelBuilder.Entity<MemberGroupResult>()
                .Property(e => e.AppraiseGroup)
                .IsUnicode(false);

            modelBuilder.Entity<MemberGroupResult>()
                .Property(e => e.AppraiseDesc)
                .IsUnicode(false);

            modelBuilder.Entity<MemberGroupResult>()
                .Property(e => e.AppraiseNo)
                .IsUnicode(false);

            modelBuilder.Entity<MemberSupport>()
                .Property(e => e.mrNumber)
                .IsUnicode(false);

            modelBuilder.Entity<MemberSupport>()
                .Property(e => e.SupportYear)
                .IsUnicode(false);

            modelBuilder.Entity<SYS_ErrorLog>()
                .Property(e => e.CreateBy)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SYS_ErrorLog>()
                .Property(e => e.LoginType)
                .IsUnicode(false);

            modelBuilder.Entity<SYS_ErrorLog>()
                .Property(e => e.URL)
                .IsUnicode(false);

            modelBuilder.Entity<SYS_ErrorLog>()
                .Property(e => e.QString)
                .IsUnicode(false);

            modelBuilder.Entity<SYS_ErrorLog>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<SYS_ErrorLog>()
                .Property(e => e.code)
                .IsUnicode(false);

            modelBuilder.Entity<SYS_ErrorLog>()
                .Property(e => e.src)
                .IsUnicode(false);

            modelBuilder.Entity<SYS_ErrorLog>()
                .Property(e => e.cat)
                .IsUnicode(false);

            modelBuilder.Entity<SYS_ErrorLog>()
                .Property(e => e.xfile)
                .IsUnicode(false);

            modelBuilder.Entity<SYS_ErrorLog>()
                .Property(e => e.xdesc)
                .IsUnicode(false);

            modelBuilder.Entity<SYS_ErrorLog>()
                .Property(e => e.adesc)
                .IsUnicode(false);

            modelBuilder.Entity<SYS_ErrorLog>()
                .Property(e => e.form_data)
                .IsUnicode(false);

            modelBuilder.Entity<SYS_ErrorLog>()
                .Property(e => e.form_method)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SYS_ErrorLog>()
                .Property(e => e.session_data)
                .IsUnicode(false);

            modelBuilder.Entity<SysUser>()
                .Property(e => e.accountNo)
                .IsUnicode(false);

            modelBuilder.Entity<SysUser>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<SysUser>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<TableBackGroup>()
                .Property(e => e.backGroupName)
                .IsUnicode(false);

            modelBuilder.Entity<TableCulture>()
                .Property(e => e.CultureName)
                .IsUnicode(false);

            modelBuilder.Entity<TableGroup>()
                .Property(e => e.GroupNo)
                .IsUnicode(false);

            modelBuilder.Entity<TableGroup>()
                .Property(e => e.GroupName)
                .IsUnicode(false);

            modelBuilder.Entity<TableMemberType>()
                .Property(e => e.membertypename)
                .IsUnicode(false);

            modelBuilder.Entity<TableNumber>()
                .Property(e => e.MemberNo)
                .IsUnicode(false);

            modelBuilder.Entity<TablePlan>()
                .Property(e => e.PlanName)
                .IsUnicode(false);
        }
    }
}
