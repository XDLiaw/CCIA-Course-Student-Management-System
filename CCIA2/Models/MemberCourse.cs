using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCIA2.Models
{
    [Table("MemberCourse")]
    public class MemberCourse
    {
        [Key]
        public int sqno { get; set; }

        [ForeignKey("course")]
        public int CourseSqno { get; set; }

        [Display(Name = "課程")]
        public virtual Course course { get; set; }

        [ForeignKey("member")]
        public int mrSqno { get; set; }

        [StringLength(50)]
        public string mrNumber { get; set; }

        [Display(Name = "會員")]
        public virtual Member member { get; set; }

        [Display(Name = "是否出席")]
        public string IsAttend { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual ICollection<MemberCourseAttchFile> memberCourseAttachFiles { get; set; }

        public MemberCourse()
        {
            this.memberCourseAttachFiles = new List<MemberCourseAttchFile>();
        }
    }
}