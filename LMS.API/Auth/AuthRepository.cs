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
        private AuthContext _ctx;

        private UserManager<IdentityUser> _userManager;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(User userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.EmailAddress
            };

            // Save in AspNetUsers
            var result = await _userManager.CreateAsync(user, userModel.Password);

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
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}