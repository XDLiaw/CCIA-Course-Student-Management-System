using CCIA2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCIA2.Controllers
{
    public class SysUserController : Controller
    {
        private CCIAContext db = new CCIAContext();


        //
        // GET: /SysUser/

        public ActionResult Index()
        {
            return View(db.SysUser.ToList());
        }

    }
}
