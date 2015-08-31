// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdminUserTree.cs" company="">
//   
// </copyright>
// <summary>
//   The admin user tree.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// The admin user tree.
    /// </summary>
    public class AdminUserTree
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client name.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        public List<User> Users { get; set; }

        #endregion
    }
}