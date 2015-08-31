// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LMSAuthorizeAttribute.cs" company="">
//   
// </copyright>
// <summary>
//   The lms authorize attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Infrastructure
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    /// <summary>
    /// The lms authorize attribute.
    /// </summary>
    public class LMSAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// The is authorized.
        /// </summary>
        /// <param name="actionContext">
        /// The action context.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            IIdentity user = HttpContext.Current.User.Identity;
            if (user.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)user;
                IEnumerable<Claim> claims = identity.Claims;
                string userRole = claims.First(c => c.Type == "role").Value;

                if (this.Role == userRole)
                {
                    return true;
                }

                return false;
            }

            return base.IsAuthorized(actionContext);
        }
    }
}