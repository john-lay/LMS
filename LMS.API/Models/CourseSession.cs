// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CourseSession.cs" company="">
//   
// </copyright>
// <summary>
//   The course session.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The course session.
    /// </summary>
    [Table("CourseSession")]
    public class CourseSession
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the course.
        /// </summary>
        public virtual Course Course { get; set; }

        /// <summary>
        /// Gets or sets the course id.
        /// </summary>
        [ForeignKey("Course")]
        public int CourseId { get; set; }

        /// <summary>
        /// Gets or sets the course session id.
        /// </summary>
        [Key]
        public int CourseSessionId { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is rolling.
        /// </summary>
        public bool IsRolling { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTime StartDate { get; set; }

        #endregion
    }
}