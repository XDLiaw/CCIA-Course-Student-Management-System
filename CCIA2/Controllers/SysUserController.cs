﻿using CCIA2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCIA2.Controllers
{
    [Authorize]
    [SessionExpire]
    public class SysUserController : Controller
    {
        private CCIAContext db = new CCIAContext();


        //
        // GET: /SysUser/

        public ActionResult Index()
        {
            return View(db.SysUser.ToList());
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
