using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.API.Models
{
    [Table("CourseSession")]
    public class CourseSession
    {
        [Key]
        public int CourseSessionId { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsRolling { get; set; }
    }
}