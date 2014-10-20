using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace LMS.API.Infrastructure
{
    public class LMSAuthorizeAttribute : AuthorizeAttribute
    {
        public string Role { get; set; }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var user = HttpContext.Current.User.Identity;
            if (user.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)user;
                IEnumerable<Claim> claims = identity.Claims;
                string userRole = claims.First(c => c.Type == "role").Value;

                if (Role == userRole)
                {
                    return true;
                }

                return false;
            }

            return base.IsAuthorized(actionContext);
        }
    }
}