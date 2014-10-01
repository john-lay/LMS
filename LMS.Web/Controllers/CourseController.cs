using LMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Web.Controllers
{
    public class CourseController : LMSBaseController
    {
        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var model = new CourseViewModel 
            {
                Id = id,
                CourseTypeList = GetCourseTypeList()
            };

            return View(model);
        }

        private IEnumerable<SelectListItem> GetCourseTypeList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = string.Empty, Text = "Please select the type..." },
                new SelectListItem { Value = "rtb", Text = "Read Tick Box" },
                //new SelectListItem { Value = "elearning", Text = "E-Learning" },
            };
        }
    }
}