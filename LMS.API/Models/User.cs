// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="">
//   
// </copyright>
// <summary>
//   The user.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The user.
    /// </summary>
    [Table("User")]
    public class User
    {
        // [ForeignKey("ASPNetUsers")]
        #region Public Properties

        /// <summary>
        /// Gets or sets the asp net user id.
        /// </summary>
        public string ASPNetUserId { get; set; }

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
        /// Gets or sets the email address.
        /// </summary>
        [StringLength(50)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [StringLength(50)]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets a value indicating whether is locked.
        /// </summary>
        public bool IsLocked
        {
            get
            {
                return this.Locked != null;
            }
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [StringLength(50)]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the locked.
        /// </summary>
        public DateTime? Locked { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        [StringLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        #endregion
    }
}