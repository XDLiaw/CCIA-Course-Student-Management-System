using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CCIA2.Models
{
    [Table("DbFile")]
    public class DbFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int sqno { get; set; }

        [StringLength(500)]
        public string fileName { get; set; }
        
        public string contentType { get; set; }

        public byte[] content { get; set; }

        public int contentLength { get; set; }

        public DateTime? createDate { get; set; }
    }
}