using CCIA2.Helper;
using CCIA2.Models;
using CCIA2.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CCIA2.Controllers
{
    public class AccountController : Controller
    {
        private CCIAContext db = new CCIAContext();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel form)
        {
            if (ModelState.IsValid)
            {
                if (validateGoogleReCaptcha() || validateCaptchaImg(form.captchaCode)) 
                {
                    //驗證資料庫登入                
                    List<SysUser> query = db.SysUser.Where(r => r.accountNo == form.accountNo && r.password == form.password).ToList();
                    if (query.Count() == 1)
                    {
                        Session["user"] = query[0];

                        bool isPersistent = false;//如果票證將存放於持續性 Cookie 中 (跨瀏覽器工作階段儲存)，則為 true，否則為 false。 如果票證是存放於 URL 中，則忽略這個值。
                        string userData = "";//可放使用者自訂的內容

                        //寫cookie
                        //使用 Cookie 名稱、版本、到期日、核發日期、永續性和使用者特定的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。 此 Cookie 路徑設定為在應用程式的組態檔中建立的預設值。
                        //使用 Cookie 名稱、版本、目錄路徑、核發日期、到期日期、永續性和使用者定義的資料，初始化 FormsAuthenticationTicket 類別的新執行個體。
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                            query[0].accountNo,//使用者ID
                            DateTime.Now,//核發日期
                            DateTime.Now.AddMinutes(1800),//到期日期 30分鐘 
                            isPersistent,//永續性
                            userData,//使用者定義的資料
                            FormsAuthentication.FormsCookiePath);

                        string encTicket = FormsAuthentication.Encrypt(ticket);
                        Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                        if (form.ReturnUrl != null)
                        {
                            return Redirect(form.ReturnUrl.ToString());
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else 
                    {
                        ViewBag.ErrorMessage = "帳號或密碼錯誤登入失敗";
                    }
                }
                else 
                {
                    ViewBag.ErrorMessage = "驗證碼錯誤，請重新輸入";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "資料格式有誤，請重新輸入!";
            }

            return View(form);
        }

        private bool validateGoogleReCaptcha()
        {
            string response = Request["g-recaptcha-response"];
            string secret = ConfigurationManager.AppSettings["reCAPTCHASecret"];
            string reply = new WebClient().DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            CaptchaResponse captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            if (captchaResponse.Success)
            {
                return true;
            }
            else
            {
                var error = captchaResponse.ErrorCodes[0].ToLower();
                switch (error)
                {
                    case ("missing-input-secret"):
                        ViewBag.Message = "The secret parameter is missing.";
                        break;
                    case ("invalid-input-secret"):
                        ViewBag.Message = "The secret parameter is invalid or malformed.";
                        break;
                    case ("missing-input-response"):
                        ViewBag.Message = "The response parameter is missing.";
                        break;
                    case ("invalid-input-response"):
                        ViewBag.Message = "The response parameter is invalid or malformed.";
                        break;
                    default:
                        ViewBag.Message = "Error occured. Please try again";
                        break;
                }
                return false;
            }
        }

        private bool validateCaptchaImg(string inputCode)
        {
            string code = Session["CAPTCHA"] as string;
            return string.Equals(code, inputCode, StringComparison.OrdinalIgnoreCase);
        }

        public void GenerateCaptchaImage()
        {
            CAPTCHAImageGenerater generater = new CAPTCHAImageGenerater();
            string code = generater.generateCode();
            Session["CAPTCHA"] = code;
            Bitmap bitmap = generater.createCodeImage(code);
            bitmap.Save(Response.OutputStream, ImageFormat.Gif);
            Response.End();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            //or Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
