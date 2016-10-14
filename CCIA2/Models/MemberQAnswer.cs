using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCIA2.Models
{
    [Table("MemberQAnswers")]
    public class MemberQAnswer
    {
        [Key]
        public int sqno { get; set; }

        [ForeignKey("member")]
        public int mrSqno { get; set; }

        [Display(Name = "會員")]
        public virtual Member member { get; set; }

        [ForeignKey("question")]
        public int qsqno { get; set; }

        [Display(Name="問題")]
        public virtual Question question { get; set; }

        [Display(Name = "答案")]
        public string ans { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}