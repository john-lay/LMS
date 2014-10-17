using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Web.Controllers
{
    public class HomeController : LMSBaseController
    {
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }
    }
}