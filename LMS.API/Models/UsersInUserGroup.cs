// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersInUserGroup.cs" company="">
//   
// </copyright>
// <summary>
//   The users in user group.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The users in user group.
    /// </summary>
    [Table("UsersInUserGroup")]
    public class UsersInUserGroup
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Gets or sets the user group.
        /// </summary>
        public virtual UserGroup UserGroup { get; set; }

        /// <summary>
        /// Gets or sets the user group id.
        /// </summary>
        [Key]
        [Column(Order = 0)]
        [ForeignKey("UserGroup")]
        public int UserGroupId { get; set; }

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