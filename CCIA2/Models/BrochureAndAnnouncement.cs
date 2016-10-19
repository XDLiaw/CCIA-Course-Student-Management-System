using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCIA2.Models
{
    [Table("BrochureAndAnnouncement")]
    public class BrochureAndAnnouncement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sqno { get; set; }

        [Display(Name = "標題")]
        [StringLength(500)]
        public string title { get; set; }

        [Display(Name = "摘要")]
        [StringLength(500)]
        public string Abstract { get; set; }

        [Display(Name = "內容")]
        [AllowHtml]
        [Column("infoContent")]
        public string content { get; set; }

        [Display(Name = "附加檔")]
        [ForeignKey("dbFile")]
        public int? dbFileSqno { get; set; }

        [Display(Name = "附加檔")]
        public virtual DbFile dbFile { get; set; }

        [Display(Name = "上傳檔案")]
        [NotMapped]
        public HttpPostedFileBase uploadFile { get; set; }

        [Display(Name = "型態")]
        public string type { get; set; }
        public const string TYPE_BROCHURE = "BROCHURE";
        public const string TYPE_ANNOUNCEMENT = "ANNOUNCEMENT";

        [Display(Name = "開始日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? startDate { get; set; }

        [Display(Name = "結束日期")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? endDate { get; set; }

        public DateTime? createDate { get; set; }

        public void storeFile()
        {
            if (this.uploadFile != null && this.uploadFile.ContentLength > 0)
            {
                if (this.dbFile == null)
                {
                    this.dbFile = new DbFile();
                }

                this.dbFile.fileName = System.IO.Path.GetFileName(this.uploadFile.FileName);
                this.dbFile.contentType = this.uploadFile.ContentType;
                this.dbFile.contentLength = this.uploadFile.ContentLength;
                using (var reader = new System.IO.BinaryReader(this.uploadFile.InputStream))
                {
                    this.dbFile.content = reader.ReadBytes(this.uploadFile.ContentLength);
                }
            }
        }
    }
}