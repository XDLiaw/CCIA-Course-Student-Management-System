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

        [NotMapped]
        public string IsAttendString
        {
            get
            {
                if (this.IsAttend == "Y") return "出席";
                else if (this.IsAttend == "N") return "缺席";
                else return "";
            }
        }

        public DateTime CreateDate { get; set; }

        public virtual ICollection<MemberCourseAttchFile> memberCourseAttachFiles { get; set; }

        [NotMapped]
        public string attachFilesString
        {
            get
            {
                string str = "";
                this.memberCourseAttachFiles.ToList().ForEach(x => str += (x.ShowAttchFileName + " "));
                return str;
            }
        }

        public MemberCourse()
        {
            this.memberCourseAttachFiles = new List<MemberCourseAttchFile>();
        }
    }
}