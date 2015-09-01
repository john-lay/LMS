// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleAuthorizationServerProvider.cs" company="">
//   
// </copyright>
// <summary>
//   The simple authorization server provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Auth
{
    using System.Data.Entity;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using LMS.API.Contexts;
    using LMS.API.Models;

    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.Owin.Security.OAuth;

    /// <summary>
    /// The simple authorization server provider.
    /// </summary>
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// The grant resource owner credentials.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            string clientId, roleName = string.Empty;
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (var repo = new AuthRepository())
            {
                IdentityUser user = await repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                clientId = this.GetClientIdFromIdentityUser(user);
                roleName = this.GetRoleNameFromIdentityUser(user);
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("username", context.UserName));
            identity.AddClaim(new Claim("role", roleName));
            identity.AddClaim(new Claim("clientId", clientId));

            context.Validated(identity);
        }

        /// <summary>
        /// The validate client authentication.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        /// <summary>
        /// The get client id from identity user.
        /// </summary>
        /// <param name="identityUser">
        /// The identity user.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetClientIdFromIdentityUser(IdentityUser identityUser)
        {
            string clientId = string.Empty;

            using (var db = new LMSContext())
            {
                User user = db.Users.First(u => u.ASPNetUserId == identityUser.Id);

                clientId = user.ClientId.ToString(CultureInfo.InvariantCulture);
            }

            return clientId;
        }

        /// <summary>
        /// The get role name from identity user.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetRoleNameFromIdentityUser(IdentityUser user)
        {
            string roleName = string.Empty;

            if (user.Roles.Count == 1)
            {
                using (var db = new AuthContext())
                {
                    IDbSet<IdentityRole> allRoles = db.Roles;
                    foreach (IdentityUserRole role in user.Roles)
                    {
                        roleName = allRoles.Find(role.RoleId).Name;
                    }
                }
            }

            return roleName;
        }
    }
}