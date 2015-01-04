using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Web.Controllers
{
    public class ContactController : LMSBaseController
    {
        // GET: Contact/Email
        public ActionResult Email()
        {
            return View();
        }
    }
}