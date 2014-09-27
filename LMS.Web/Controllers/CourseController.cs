using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Web.Controllers
{
    public class CourseController : Controller
    {
        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            ViewBag.courseId = id;
            return View();
        }
    }
}