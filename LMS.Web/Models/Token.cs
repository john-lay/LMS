// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Token.cs" company="">
//   
// </copyright>
// <summary>
//   The token.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Models
{
    /// <summary>
    /// The token.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Gets or sets the access_token.
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// Gets or sets the expires_in.
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// Gets or sets the token_type.
        /// </summary>
        public string token_type { get; set; }
    }
}