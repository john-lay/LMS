// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CourseViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The course view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// The course view model.
    /// </summary>
    public class CourseViewModel
    {
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        [Required]
        public HttpPostedFileBase Content { get; set; }

        /// <summary>
        /// Gets or sets the course id.
        /// </summary>
        [Required]
        public int CourseId { get; set; }

        /// <summary>
        /// Gets or sets the course type.
        /// </summary>
        [Required]
        [Display(Name = "Course Type")]
        public string CourseType { get; set; }

        /// <summary>
        /// Gets or sets the course type list.
        /// </summary>
        public IEnumerable<SelectListItem> CourseTypeList { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        [StringLength(500)]
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the objectives.
        /// </summary>
        [StringLength(500)]
        public string Objectives { get; set; }
    }
}