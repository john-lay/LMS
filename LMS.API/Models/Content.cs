// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Content.cs" company="">
//   
// </copyright>
// <summary>
//   The content.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The content.
    /// </summary>
    [Table("Content")]
    public class Content
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContentId { get; set; }

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
        /// Gets or sets the description.
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the resource.
        /// </summary>
        [StringLength(512)]
        public string Resource { get; set; }

        #endregion
    }
}