using CCIA2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CCIA2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private CCIAContext db = new CCIAContext();
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (Session[SessionKey.USER] == null)
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

    }
}
