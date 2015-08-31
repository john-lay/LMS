// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthRepository.cs" company="">
//   
// </copyright>
// <summary>
//   The auth repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Auth
{
    using System;
    using System.Threading.Tasks;

    using LMS.API.Contexts;
    using LMS.API.Controllers;
    using LMS.API.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// The auth repository.
    /// </summary>
    public class AuthRepository : IDisposable
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly AuthContext context;

        /// <summary>
        /// The user manager.
        /// </summary>
        private readonly UserManager<IdentityUser> userManager;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthRepository"/> class.
        /// </summary>
        public AuthRepository()
        {
            this.context = new AuthContext();
            this.userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(this.context));
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            this.context.Dispose();
            this.userManager.Dispose();
        }

        /// <summary>
        /// The find user.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await this.userManager.FindAsync(userName, password);

            return user;
        }

        /// <summary>
        /// The register admin.
        /// </summary>
        /// <param name="userModel">
        /// The user model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IdentityResult> RegisterAdmin(User userModel)
        {
            var user = new IdentityUser
                           {
                               UserName = userModel.EmailAddress
                           };

            // Save in AspNetUsers
            IdentityResult result = await this.userManager.CreateAsync(user, userModel.Password);

            if (!result.Succeeded)
            {
                return result;
            }

            // Add to user role
            this.userManager.AddToRole(user.Id, "Admin");

            // Save in User table
            var repo = new UsersController();
            userModel.ASPNetUserId = user.Id;
            userModel.Password = null;
            User[] users =
            {
                userModel
            };
            repo.CreateUser(users);

            return result;
        }

        /// <summary>
        /// The register user.
        /// </summary>
        /// <param name="userModel">
        /// The user model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IdentityResult> RegisterUser(User userModel)
        {
            var user = new IdentityUser
                           {
                               UserName = userModel.EmailAddress
                           };

            // Save in AspNetUsers
            IdentityResult result = await this.userManager.CreateAsync(user, userModel.Password);

            if (!result.Succeeded)
            {
                return result;
            }

            // Add to user role
            this.userManager.AddToRole(user.Id, "User");

            // Save in User table
            var repo = new UsersController();
            userModel.ASPNetUserId = user.Id;
            userModel.Password = null;
            User[] users =
            {
                userModel
            };
            repo.CreateUser(users);

            return result;
        }

        /// <summary>
        /// The users controller.
        /// </summary>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        private object UsersController()
        {
            throw new NotImplementedException();
        }
    }
}