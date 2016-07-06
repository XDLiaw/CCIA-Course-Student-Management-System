using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCIA2.Models
{
    [Table("Course")]
    public class Course
    {
        public Course()
        {
            this.teachers = new HashSet<CourseTeacherRelation>();
        }

        [Key]
        public int sqno { get; set; }

        [ForeignKey("courseClass")]
        public int courseClassSqno { get; set; }

        public CourseClass courseClass { get; set; }

        [Display(Name = "主題")]
        [StringLength(50)]
        public string topic { get; set; }

        [Display(Name = "日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime day { get; set; }

        [Display(Name = "時間(起)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime startTime { get; set; }

        [Display(Name = "時間(訖)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime endTime { get; set; }

        [Display(Name = "時數")]
        [DisplayFormat(DataFormatString = "{0:F1}")]
        public double? hour { get; set; }

        //[Display(Name = "講師")]
        //public virtual ICollection<CourseTeacher> teachers { get; set; }

        public virtual ICollection<CourseTeacherRelation> teachers { get; set; }

        [Display(Name = "講題")]
        [StringLength(50)]
        public string title { get; set; }

        public int? maxStudentNum { get; set; }

        [Display(Name = "地點")]
        [StringLength(50)]
        public string location { get; set; }

        [Display(Name = "開放給經紀中介學員")]
        public bool memberType1 { get; set; }

        [Display(Name = "開放給歷屆學員")]
        public bool memberType2 { get; set; }

        [Display(Name = "開放給一般會員")]
        public bool memberType3 { get; set; }

    }
}