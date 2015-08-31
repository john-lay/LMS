// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersInCourseSession.cs" company="">
//   
// </copyright>
// <summary>
//   The users in course session.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The users in course session.
    /// </summary>
    [Table("UsersInCourseSession")]
    public class UsersInCourseSession
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the course session.
        /// </summary>
        public virtual CourseSession CourseSession { get; set; }

        /// <summary>
        /// Gets or sets the course session id.
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [ForeignKey("CourseSession")]
        public int CourseSessionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether learning complete.
        /// </summary>
        public bool LearningComplete { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [ForeignKey("User")]
        public int UserId { get; set; }

        #endregion
    }
}