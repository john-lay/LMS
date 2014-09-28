using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Web.Models
{
    public class CourseViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Course Type")]
        public string CourseType { get; set; }

        public IEnumerable<SelectListItem> CourseTypeList { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(500)]
        public string Objectives { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [Required]
        public HttpPostedFileBase Content { get; set; }
    }
}