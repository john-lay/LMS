using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LMS.API.Auth
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            string roleName = string.Empty;
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (AuthRepository _repo = new AuthRepository())
            {
                IdentityUser user = await _repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                roleName = GetRoleNameFromIdentityUser(user);
            }
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("username", context.UserName));
            identity.AddClaim(new Claim("role", roleName));

            context.Validated(identity);

        }

        private string GetRoleNameFromIdentityUser(IdentityUser user)
        {
            string roleName = string.Empty;

            if (user.Roles.Count == 1)
            {
                using (var db = new LMS.API.Contexts.AuthContext())
                {
                    var allRoles = db.Roles;
                    foreach (var role in user.Roles)
                    {
                        roleName = allRoles.Find(role.RoleId).Name;
                    }
                }
            }

            return roleName;
        }
    }
}