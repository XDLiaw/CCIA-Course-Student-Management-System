using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCIA2.Models
{
    [Table("CourseTeacher")]
    public class CourseTeacher
    {
        public CourseTeacher()
        {
            this.courses = new HashSet<CourseTeacherRelation>();
        }

        [Key]
        public int sqno { get; set; }

        //[ForeignKey("course")]
        //public int courseSqno { get; set; }

        //public virtual Course course { get; set; }

        public virtual ICollection<CourseTeacherRelation> courses { get; set; }

        [Display(Name = "講師姓名")]
        [StringLength(50)]
        public string name { get; set; }

        [Display(Name = "所屬單位")]
        [StringLength(50)]
        public string orgName { get; set; }

        [Display(Name = "講師簡介")]
        [AllowHtml]
        public string introduction { get; set; }

        [Display(Name = "講師照片")]
        public byte[] photo { get; set; }

        [NotMapped]
        public string photoBase64
        {
            get
            {
                if (this.photo != null)
                {
                    return System.Convert.ToBase64String(this.photo);
                }
                else
                {
                    return null;
                }
            }
        }

        [NotMapped]
        public HttpPostedFileBase uploadPhoto { get; set; }

        public void savingPhoto()
        {
            if (this.uploadPhoto != null && this.uploadPhoto.ContentLength > 0)
            {
                this.photoContentType = this.uploadPhoto.ContentType;
                this.photoName = this.uploadPhoto.FileName;
                using (Stream inputStream = this.uploadPhoto.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }

                    this.photo = memoryStream.ToArray();
                }
            }
        }

        [StringLength(100)]
        public string photoName { get; set; }

        [StringLength(100)]
        public string photoContentType { get; set; }

    }
}