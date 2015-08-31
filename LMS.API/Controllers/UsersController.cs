// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersController.cs" company="">
//   
// </copyright>
// <summary>
//   The users controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Controllers
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Script.Serialization;

    using LMS.API.Contexts;
    using LMS.API.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// The users controller.
    /// </summary>
    public class UsersController : ApiBaseController
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LMSContext db = new LMSContext();

        /// <summary>
        /// The create user.
        /// </summary>
        /// <param name="users">
        /// The users.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        public async Task<IHttpActionResult> CreateUser([FromBody] User[] users)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            foreach (User user in users)
            {
                // prevent the creation of users with an already registered email address
                if (UserExists(user.EmailAddress))
                {
                    return this.BadRequest(this.ModelState);
                }

                this.db.Users.Add(user);
                this.db.SaveChanges();
            }

            return this.Ok(users);
        }

        /// <summary>
        /// The delete user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = this.db.Users.Find(id);
            string aspNetUserId = user.ASPNetUserId;

            if (user == null)
            {
                return this.NotFound();
            }

            this.db.Users.Remove(user);
            this.db.SaveChanges();

            // delete from [AspNetUser] table
            using (var authDb = new AuthContext())
            {
                IdentityUser identityUser = authDb.Users.Find(aspNetUserId);
                authDb.Users.Remove(identityUser);
                authDb.SaveChanges();
            }

            return this.Ok(user);
        }

        /// <summary>
        /// The get admins.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<string> GetAdmins()
        {
            var authDb = new AuthContext();
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(authDb));
            var tree = new List<AdminUserTree>();

            // all [AspNetUsers] in the "Admin" role
            string[] adminIdentities = authDb.Users.ToArray().Where(x => userManager.IsInRole(x.Id, "Admin")).Select(x => x.Id).ToArray();

            // join clients and users
            var query = (from clients in this.db.Clients
                         join users in this.db.Users on clients.ClientId equals users.ClientId
                         where adminIdentities.Contains(users.ASPNetUserId)
                         select new
                            {
                                clients.ClientId, 
                                ClientName = clients.Name, 
                                users.FirstName, 
                                users.LastName, 
                                users.UserId
                            })
                            .ToArray();

            // add this to a tree so we can convert to kendo
            foreach (var node in query)
            {
                // check if client exist in tree
                if (!tree.Any(x => x.ClientId == node.ClientId))
                {
                    // add client to tree and initialise user list
                    tree.Add(new AdminUserTree
                                 {
                                     ClientId = node.ClientId, 
                                     ClientName = node.ClientName, 
                                     Users = new List<User>()
                                 });
                }

                // add user to tree
                AdminUserTree clientNode = tree.First(c => c.ClientId == node.ClientId);
                clientNode.Users.Add(new User
                                         {
                                             FirstName = node.FirstName, 
                                             LastName = node.LastName, 
                                             UserId = node.UserId
                                         });
            }

            // normalize AdminUserTree for Kendo Tree
            var returnKendo = tree.Select(c => new
                                                   {
                                                       id = c.ClientId, 
                                                       text = c.ClientName, 
                                                       expanded = true, 
                                                       spriteCssClass = "group", 
                                                       items = c.Users
                                                           .Select(u => new
                                                            {
                                                                id = u.UserId, 
                                                                text = u.FirstName + " " + u.LastName
                                                            })
                                                            .ToArray()
                                                   });

            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(returnKendo);
        }

        /// <summary>
        /// The get user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = this.db.Users.Find(id);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.Ok(new
                               {
                                   user.FirstName, user.LastName
                               });
        }

        /// <summary>
        /// The get user profile by email.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        public async Task<IHttpActionResult> GetUserProfileByEmail()
        {
            string emailAddress = HttpContext.Current.Request.Form[0];

            var query = (from u in this.db.Users
                         join c in this.db.Clients on u.ClientId equals c.ClientId
                         where u.EmailAddress == emailAddress
                         select new
                                    {
                                        c.ClientId, c.Name, u.UserId, u.ASPNetUserId, u.FirstName, u.LastName
                                    }).ToArray();

            // get user role
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(new AuthContext()));
            string role = userManager.GetRoles(query.First().ASPNetUserId)[0];

            var result = query.Select(x => new
                                               {
                                                   x.ClientId, 
                                                   ClientName = x.Name, 
                                                   x.UserId, 
                                                   x.FirstName, 
                                                   x.LastName, 
                                                   Role = role
                                               }).ToArray();

            return this.Ok(result);
        }

        // [HttpGet]
        /// <summary>
        /// The get users by client.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IHttpActionResult> GetUsersByClient(int id)
        {
            IEnumerable<User> users = this.db.Users
                .Where(u => u.ClientId == id)
                .Select(u => new
                {
                    // use projection here to create an anonymous object. Then below return a new User object as a subset
                    u.ClientId, u.UserId, u.FirstName, u.LastName, u.EmailAddress
                                                                                                     
                    // ClientObj = u.Client,
                    // Password = u.Password
                })
                .ToList()
                .Select(u => new User
                {
                    ClientId = u.ClientId, 
                    UserId = u.UserId, 
                    FirstName = u.FirstName, 
                    LastName = u.LastName, 
                    EmailAddress = u.EmailAddress
                });

            return this.Ok(users);
        }

        /// <summary>
        /// The update user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPatch]
        public async Task<IHttpActionResult> UpdateUser(int id, [FromBody] User user)
        {
            user.ClientId = this.ClientId;

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != user.UserId)
            {
                return this.BadRequest();
            }

            // grab the user details from a existing record. Don't want to overwrite AspNetUserId or email
            User existingUser = this.db.Users.First(u => u.UserId == user.UserId);

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;

            try
            {
                this.db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return this.NotFound();
                }

                throw;
            }

            return this.StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// The user exists.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool UserExists(int id)
        {
            return this.db.Users.Count(e => e.UserId == id) > 0;
        }

        /// <summary>
        /// The user exists.
        /// </summary>
        /// <param name="email">
        /// The email.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool UserExists(string email)
        {
            return this.db.Users.Count(e => e.EmailAddress == email) > 0;
        }
    }
}