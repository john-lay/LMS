// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserGroupNode.cs" company="">
//   
// </copyright>
// <summary>
//   The user group node.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    /// <summary>
    /// The user group node.
    /// </summary>
    public class UserGroupNode
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the user group.
        /// </summary>
        public UserGroup UserGroup { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        public User[] Users { get; set; }

        #endregion
    }
}