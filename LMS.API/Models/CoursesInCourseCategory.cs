// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CoursesInCourseCategory.cs" company="">
//   
// </copyright>
// <summary>
//   The courses in course category.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The courses in course category.
    /// </summary>
    [Table("CoursesInCourseGroup")]
    public class CoursesInCourseCategory
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the course.
        /// </summary>
        public virtual Course Course { get; set; }

        /// <summary>
        /// Gets or sets the course category.
        /// </summary>
        public virtual CourseCategory CourseCategory { get; set; }

        /// <summary>
        /// Gets or sets the course category id.
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [ForeignKey("CourseCategory")]
        public int CourseCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the course id.
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [ForeignKey("Course")]
        public int CourseId { get; set; }

        #endregion
    }
}