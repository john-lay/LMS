using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.API.Models
{
    [Table("CoursesInCourseGroup")]
    public class CoursesInCourseCategory
    {
        [Key, Column(Order = 0)]
        [ForeignKey("CourseCategory")]
        public int CourseCategoryId { get; set; }

        public virtual CourseCategory CourseCategory { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }
    }
}