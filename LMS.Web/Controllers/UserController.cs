using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Web.Controllers
{
    public class UserController : LMSBaseController
    {
        // GET: User
        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult ManageAdmin()
        {
            return View();
        }

        public ActionResult Group()
        {
            return View();
        }
    }
}