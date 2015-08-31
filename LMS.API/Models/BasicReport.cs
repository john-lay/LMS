// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BasicReport.cs" company="">
//   
// </copyright>
// <summary>
//   The basic report.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    using System;

    /// <summary>
    /// The basic report.
    /// </summary>
    public class BasicReport
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the course category name.
        /// </summary>
        public string CourseCategoryName { get; set; }

        /// <summary>
        /// Gets or sets the course name.
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// Gets or sets the course type.
        /// </summary>
        public string CourseType { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is rolling.
        /// </summary>
        public bool IsRolling { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether learning complete.
        /// </summary>
        public bool LearningComplete { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the user group name.
        /// </summary>
        public string UserGroupName { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId { get; set; }

        #endregion
    }
}