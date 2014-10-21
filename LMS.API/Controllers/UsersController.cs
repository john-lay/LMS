using LMS.API.Contexts;
using LMS.API.Infrastructure;
using LMS.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace LMS.API.Controllers
{
    public class UsersController : ApiBaseController
    {
        private LMSContext db = new LMSContext();

        // GET: api/Users/5
        [HttpGet]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new { FirstName = user.FirstName, LastName = user.LastName});
        }

        [HttpPost]
        public IHttpActionResult GetUserProfileByEmail()
        {
            string emailAddress = HttpContext.Current.Request.Form[0];

            var query = (from u in db.Users
                        join c in db.Clients on u.ClientId equals c.ClientId
                         where u.EmailAddress == emailAddress
                        select new 
                        {
                            c.ClientId,
                            c.Name,
                            u.UserId,
                            u.FirstName,
                            u.LastName
                        }).ToArray();

            var result = query.Select(x => new 
            {
                ClientId = x.ClientId,
                ClientName = x.Name,
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName
            }).ToArray();

            return Ok(result);
        }

        //[HttpGet]
        public IHttpActionResult GetUsersByClient(int id)
        {
            IEnumerable<User> users = db.Users
                .Where(u => u.ClientId == id)
                .Select(u => new // use projection here to create an anonymous object. Then below return a new User object as a subset
                { 
                    ClientId = u.ClientId,
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    EmailAddress = u.EmailAddress
                    //ClientObj = u.Client,
                    //Password = u.Password
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

            return Ok(users);
        }

        //[LMSAuthorize(Role = "SuperAdmin")]
        public string GetAdmins()
        {
            var authDb = new AuthContext();
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(authDb));
            List<AdminUserTree> tree = new List<AdminUserTree>();

            // all [AspNetUsers] in the "Admin" role
            string[] adminIdentities = authDb.Users
                .ToArray()
                .Where(x => userManager.IsInRole(x.Id, "Admin"))
                .Select(x => x.Id)
                .ToArray();

            // join clients and users
            var query = (from clients in db.Clients
                         join users in db.Users on clients.ClientId equals users.ClientId
                         where adminIdentities.Contains(users.ASPNetUserId)
                         select new 
                         {
                            ClientId = clients.ClientId,
                            ClientName = clients.Name,
                            FirstName = users.FirstName,
                            LastName = users.LastName,
                            UserId = users.UserId
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
                var clientNode = tree.First(c => c.ClientId == node.ClientId);
                clientNode.Users.Add(new User
                {
                    FirstName = node.FirstName,
                    LastName = node.LastName,
                    UserId = node.UserId
                });
            }

            // normalize AdminUserTree for Kendo Tree
            var returnKendo = tree
                .Select(c => new
                {
                    id = c.ClientId,
                    text = c.ClientName,
                    expanded = true,
                    spriteCssClass = "group",
                    items = c.Users.Select(u => new { id = u.UserId, text = u.FirstName + " " + u.LastName }).ToArray()
                });

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(returnKendo);
        }

        // PUT: api/Users/5
        [HttpPatch]
        public IHttpActionResult UpdateUser(int id, [FromBody]User user)
        {
            user.ClientId = this.ClientId;            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            // grab the user details from a existing record. Don't want to overwrite AspNetUserId or email
            User existingUser = db.Users.First(u => u.UserId == user.UserId);

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [HttpPost]
        public IHttpActionResult CreateUser([FromBody]User[] users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (User user in users)
            {
                // prevent the creation of users with an already registered email address
                if (UserExists(user.EmailAddress))
                {
                    return BadRequest(ModelState);
                }
                else
                { 
                    db.Users.Add(user);
                    db.SaveChanges();
                }                
            }

            return Ok(users);
        }

        // DELETE: api/Users/5
        [HttpDelete]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            var aspNetUserId = user.ASPNetUserId;

            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            // delete from [AspNetUser] table
            using (var authDb = new AuthContext())
            {
                IdentityUser identityUser = authDb.Users.Find(aspNetUserId);
                authDb.Users.Remove(identityUser);
                authDb.SaveChanges();
            }

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }

        private bool UserExists(string email)
        {
            return db.Users.Count(e => e.EmailAddress == email) > 0;
        }
    }
}