using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.API.Models
{
    [Table("CoursesInCourseGroup")]
    public class CoursesInCourseGroup
    {
        [Key, Column(Order = 0)]
        [ForeignKey("CourseGroup")]
        public int CourseGroupId { get; set; }

        public virtual CourseGroup CourseGroup { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }
    }
}