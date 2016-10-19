using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCIA2.Models
{
    [Table("BannerAndLink")]
    public class BannerAndLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sqno { get; set; }

        [Display(Name = "名稱")]
        [StringLength(500)]
        public string name { get; set; }

        [Display(Name = "URL")]
        [StringLength(1000)]
        public string url { get; set; }

        [Display(Name = "圖檔")]
        [ForeignKey("dbFile")]
        public int? dbFileSqno { get; set; }

        [Display(Name = "圖檔")]
        public virtual DbFile dbFile { get; set; }

        public string photoBase64
        {
            get
            {
                if (this.dbFile != null)
                {
                    return System.Convert.ToBase64String(this.dbFile.content);
                }
                return null;
            }
        }

        [Display(Name = "上傳檔案")]
        [NotMapped]
        public HttpPostedFileBase uploadFile { get; set; }

        [Display(Name = "型態")]
        public string type { get; set; }
        public const string TYPE_BANNER = "BANNER";
        public const string TYPE_LINK = "LINK";

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