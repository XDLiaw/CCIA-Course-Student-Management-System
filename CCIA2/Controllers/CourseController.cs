using CCIA2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCIA2.Controllers
{
    [Authorize]
    [SessionExpire]
    public class CourseController : Controller
    {
        private CCIAContext db = new CCIAContext();

        public ActionResult Index()
        {





            return View();
        }

    }
}
