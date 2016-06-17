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

        public virtual DbSet<SysUser> SysUser { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SysUser>()
                .Property(e => e.accountNo)
                .IsUnicode(false);

            modelBuilder.Entity<SysUser>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<SysUser>()
                .Property(e => e.name)
                .IsUnicode(false);
        }
    }
}
