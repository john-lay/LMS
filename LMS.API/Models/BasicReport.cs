using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.API.Models
{
    public class BasicReport
    {
        public bool LearningComplete { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string UserGroupName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsRolling { get; set; }

        public string CourseName { get; set; }

        public string CourseType { get; set; }

        public string CourseCategoryName { get; set; }
    }
}