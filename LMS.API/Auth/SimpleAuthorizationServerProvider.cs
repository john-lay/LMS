using LMS.API.Contexts;
using LMS.API.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using System.Linq;
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
            string clientId, roleName = string.Empty;
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (AuthRepository _repo = new AuthRepository())
            {
                IdentityUser user = await _repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                clientId = GetClientIdFromIdentityUser(user);
                roleName = GetRoleNameFromIdentityUser(user);
            }
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("username", context.UserName));
            identity.AddClaim(new Claim("role", roleName));
            identity.AddClaim(new Claim("clientId", clientId));

            context.Validated(identity);

        }

        private string GetClientIdFromIdentityUser(IdentityUser identityUser)
        {
            string clientId = string.Empty;

            using (var db = new LMSContext())
            {
                var user = db.Users
                    .First(u => u.ASPNetUserId == identityUser.Id);

                clientId = user.ClientId.ToString();
            }

            return clientId;
        }

        private string GetRoleNameFromIdentityUser(IdentityUser user)
        {
            string roleName = string.Empty;

            if (user.Roles.Count == 1)
            {
                using (var db = new AuthContext())
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