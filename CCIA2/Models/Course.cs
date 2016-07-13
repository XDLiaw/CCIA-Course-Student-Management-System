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

        [Display(Name="課程類別")]
        public CourseClass courseClass { get; set; }

        [Display(Name = "主題")]
        [StringLength(50)]
        public string topic { get; set; }

        [Display(Name = "日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? day { get; set; }

        [Display(Name = "時間(起)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime? startTime { get; set; }

        [Display(Name = "時間(訖)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public DateTime? endTime { get; set; }

        [Display(Name = "時數")]
        [DisplayFormat(DataFormatString = "{0:F1}")]
        public double? hour { get; set; }

        [NotMapped]
        public List<int> teacherSqnoList
        {
            get
            {
                return this.teachers.Select(t => t.teacherSqno).ToList();
            }
            set
            {
                this.teachers = new List<CourseTeacherRelation>();
                if (value != null && value.Count > 0)
                {
                    value.ForEach(sqno => this.teachers.Add(new CourseTeacherRelation() { courseSqno = this.sqno, teacherSqno = sqno }));
                }
            }
        }

        [NotMapped]
        public string teacherSqnoListString
        {
            get
            {
                return this.teachers.Select(t => t.teacherSqno).ToList().ToString();
            }
            set
            {
                this.teachers = new List<CourseTeacherRelation>();
                if (value != null)
                {
                    string[] sqnoStringList = value.Split(',');
                    if (sqnoStringList != null && sqnoStringList.Length > 0)
                    {
                        foreach (string sqnoString in sqnoStringList)
                        {
                            this.teachers.Add(new CourseTeacherRelation() { courseSqno = this.sqno, teacherSqno = Int32.Parse(sqnoString) });
                        }
                    }
                }
            }
        }

        [Display(Name = "講師")]
        public virtual ICollection<CourseTeacherRelation> teachers { get; set; }

        [Display(Name = "講題")]
        [StringLength(50)]
        public string title { get; set; }

        [Display(Name="學生人數上限")]
        public int? maxStudentNum { get; set; }

        [Display(Name = "圖像授權組人數上限")]
        public int? maxGroup1StudentNum { get; set; }

        [Display(Name = "故事行銷組人數上限")]
        public int? maxGroup2StudentNum { get; set; }

        [Display(Name = "文創科技組人數上限")]
        public int? maxGroup3StudentNum { get; set; }

        [Display(Name = "地點")]
        [StringLength(50)]
        public string location { get; set; }

        [Display(Name = "開放給經紀中介學員")]
        public bool memberType1 { get; set; }

        [Display(Name = "開放給歷屆學員")]
        public bool memberType2 { get; set; }

        [Display(Name = "開放給一般會員")]
        public bool memberType3 { get; set; }

        [Display(Name = "說明")]
        public string content { get; set; }
    }
}