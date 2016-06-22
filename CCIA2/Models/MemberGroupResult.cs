namespace CCIA2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MemberGroupResult")]
    public partial class MemberGroupResult
    {
        [Key]
        public int sqno { get; set; }

        public int? mrSqno { get; set; }

        [StringLength(50)]
        public string mrNumber { get; set; }

        [Display(Name="���q")]
        public int AppraiseStep { get; set; }

        [Display(Name="���G")]
        [StringLength(1)]
        public string AppraiseResult { get; set; }

        [Display(Name="�f�d����էO")]
        [StringLength(50)]
        public string AppraiseGroup { get; set; }

        [Display(Name="�f�d���N��")]
        public string AppraiseDesc { get; set; }

        [Display(Name="�f�d���")]
        public DateTime? AppraiseCreateDt { get; set; }

        [Display(Name="�f�d���s��")]
        [StringLength(50)]
        public string AppraiseNo { get; set; }

        public virtual Member Member { get; set; }

        public string currentStateString()
        {
            if (this.AppraiseStep == 1)
            {
                return "���f";
            }
            else if (this.AppraiseStep == 2)
            {
                return "��f";
            }
            else if (this.AppraiseStep == 3)
            {
                return "����";
            }
            else if (this.AppraiseStep == 4)
            {
                return "ú��O�Ҫ�";
            }
            return "";
        }

        public string appraiseResultString()
        {
            if (this.AppraiseStep == 1 || this.AppraiseStep == 2)
            {
                return Int32.Parse(this.AppraiseResult) > 0 ? "�q�L" : "���q�L";
            }
            if (this.AppraiseStep == 3)
            {
                if (Int32.Parse(this.AppraiseResult) == 1) return "����";
                else if (Int32.Parse(this.AppraiseResult) == 2) return "�ƨ�";
                else return "������";
            }
            if (this.AppraiseStep == 4)
            {
                return Int32.Parse(this.AppraiseResult) > 0 ? "�O" : "�_";
            }
            return "";
        }
    }
}
