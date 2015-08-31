// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CourseCategoryNode.cs" company="">
//   
// </copyright>
// <summary>
//   The course category node.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    /// <summary>
    /// The course category node.
    /// </summary>
    public class CourseCategoryNode
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public CourseCategory Category { get; set; }

        /// <summary>
        /// Gets or sets the courses.
        /// </summary>
        public Course[] Courses { get; set; }

        #endregion
    }
}