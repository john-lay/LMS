using LMS.API.Contexts;
using LMS.API.Controllers;
using LMS.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Threading.Tasks;

namespace LMS.API.Auth
{
    public class AuthRepository : IDisposable
    {
        private AuthContext context;

        private UserManager<IdentityUser> userManager;

        public AuthRepository()
        {
            context = new AuthContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));
        }

        public async Task<IdentityResult> RegisterUser(User userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.EmailAddress
            };

            // Save in AspNetUsers
            var result = await userManager.CreateAsync(user, userModel.Password);

            if (!result.Succeeded)
            {
                return result;
            }

            // Add to user role
            userManager.AddToRole(user.Id, "User");

            // Save in User table
            var repo = new UsersController();
            userModel.ASPNetUserId = user.Id;
            userModel.Password = null;
            User[] users = new User[] { userModel };
            repo.CreateUser(users);

            return result;
        }

        public async Task<IdentityResult> RegisterAdmin(User userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.EmailAddress
            };

            // Save in AspNetUsers
            var result = await userManager.CreateAsync(user, userModel.Password);

            if (!result.Succeeded)
            {
                return result;
            }

            // Add to user role
            userManager.AddToRole(user.Id, "Admin");

            // Save in User table
            var repo = new UsersController();
            userModel.ASPNetUserId = user.Id;
            userModel.Password = null;
            User[] users = new User[] { userModel };
            repo.CreateUser(users);

            return result;
        }

        private object UsersController()
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            context.Dispose();
            userManager.Dispose();
        }
    }
}