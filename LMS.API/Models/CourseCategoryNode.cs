using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.API.Models
{
    public class CourseCategoryNode
    {
        public CourseCategory Category { get; set; }

        public Course[] Courses { get; set; }
    }
}