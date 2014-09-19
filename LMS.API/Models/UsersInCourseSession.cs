using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.API.Models
{
    [Table("UsersInCourseSession")]
    public class UsersInCourseSession
    {
        [Key, Column(Order = 0)]
        [ForeignKey("CourseSession")]
        public int CourseSessionId { get; set; }

        public virtual CourseSession CourseSession { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}