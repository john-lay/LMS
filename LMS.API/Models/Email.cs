// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Email.cs" company="">
//   
// </copyright>
// <summary>
//   The email.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    /// <summary>
    /// The email.
    /// </summary>
    public class Email
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the recipients.
        /// </summary>
        public string[] Recipients { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        public string Subject { get; set; }

        #endregion
    }
}