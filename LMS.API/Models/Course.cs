// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Course.cs" company="">
//   
// </copyright>
// <summary>
//   The course.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The course.
    /// </summary>
    [Table("Course")]
    public class Course
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        public virtual Client Client { get; set; }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        [ForeignKey("Client")]
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the course id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }

        /// <summary>
        /// Gets or sets the course type.
        /// </summary>
        [StringLength(50)]
        public string CourseType { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        #endregion
    }
}