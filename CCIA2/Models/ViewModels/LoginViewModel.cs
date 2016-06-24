using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CCIA2.Models.ViewModels
{
    public class LoginViewModel
    {
        [DisplayName("登入帳號")]
        [Required(ErrorMessage = "請輸入登入帳號")]
        [StringLength(50)]
        public string accountNo { get; set; }

        [DisplayName("登入密碼")]
        [Required(ErrorMessage = "請輸入登入密碼")]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public string ReturnUrl { get; set; }
        
        [Display(Name="驗證碼")]
        public string captchaCode { get; set; }
    }
}