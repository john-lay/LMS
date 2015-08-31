// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthContext.cs" company="">
//   
// </copyright>
// <summary>
//   The auth context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Contexts
{
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// The auth context.
    /// </summary>
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthContext"/> class.
        /// </summary>
        public AuthContext()
            : base("name=LMSContext")
        {
            // this uses the "LMSContext" connection string from the config
        }
    }
}