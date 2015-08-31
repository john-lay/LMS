// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CourseCategory.cs" company="">
//   
// </copyright>
// <summary>
//   The course category.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The course category.
    /// </summary>
    [Table("CourseCategory")]
    public class CourseCategory
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
        /// Gets or sets the course category id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        [StringLength(500)]
        public string Notes { get; set; }

        #endregion
    }
}