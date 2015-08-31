// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserGroup.cs" company="">
//   
// </copyright>
// <summary>
//   The user group.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The user group.
    /// </summary>
    [Table("UserGroup")]
    public class UserGroup
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
        /// Gets or sets the name.
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        // -1 for root
        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// Gets or sets the user group id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserGroupId { get; set; }

        #endregion
    }
}