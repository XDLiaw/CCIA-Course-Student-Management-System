using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCIA2.Models
{
    [Table("Questions")]
    public class Question
    {
        [Key]
        public int sqno { get; set; }

        [ForeignKey("course")]
        public int courseSqno { get; set; }

        [Display(Name = "課程")]
        public virtual Course course { get; set; }

        [Display(Name = "主題")]
        public string title { get; set; }

        [Display(Name = "考題")]
        public string q { get; set; }

        [Display(Name = "選項1")]
        public string s1 { get; set; }

        [Display(Name = "選項2")]
        public string s2 { get; set; }

        [Display(Name = "選項3")]
        public string s3 { get; set; }

        [Display(Name = "選項4")]
        public string s4 { get; set; }

        [Display(Name = "選項")]
        public List<string> optionList
        {
            get
            {
                List<string> list = new List<string>();
                list.Add(this.s1);
                list.Add(this.s2);
                list.Add(this.s3);
                list.Add(this.s4);
                return list;
            }
        }

        [Display(Name = "答案")]
        public string ans { get; set; }

        public string years { get; set; }


    }
}