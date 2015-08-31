// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersInUserGroupsController.cs" company="">
//   
// </copyright>
// <summary>
//   The users in user groups controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Controllers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Script.Serialization;

    using LMS.API.Contexts;
    using LMS.API.Models;

    /// <summary>
    /// The users in user groups controller.
    /// </summary>
    public class UsersInUserGroupsController : ApiBaseController
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LMSContext db = new LMSContext();

        /// <summary>
        /// The get user groups and user emails.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<string> GetUserGroupsAndUserEmails()
        {
            var userGroupTree = new List<UserGroupNode>();
            var userIdList = new List<int>();

            DbSet<UserGroup> groups = this.db.UserGroups;

            foreach (UserGroup grp in groups)
            {
                IQueryable<User> query = from usersInUserGroups in this.db.UsersInUserGroups join user in this.db.Users on usersInUserGroups.UserId equals user.UserId where usersInUserGroups.UserGroupId == grp.UserGroupId && user.ClientId == this.ClientId select user;

                userGroupTree.Add(new UserGroupNode
                                      {
                                          UserGroup = grp, Users = query.ToArray()
                                      });

                userIdList.AddRange(query.Select(x => x.UserId));
            }

            // normalize userGroupTree for Kendo Tree
            var returnKendo = userGroupTree.Select(g => new
                                                            {
                                                                id = g.UserGroup.UserGroupId, text = g.UserGroup.Name, expanded = true, spriteCssClass = "group", items = g.Users.Select(u => new
                                                                                                                                                                                                  {
                                                                                                                                                                                                      id = u.UserId, text = u.EmailAddress
                                                                                                                                                                                                  }).ToArray()
                                                            });

            // grab users for this client, who are not in a group and add them to the end of the tree
            var usersNotInGroup = this.db.Users.Where(x => x.ClientId == this.ClientId).ToArray().Where(x => userIdList.IndexOf(x.UserId) == -1).Select(u => new
                                                                                                                                                                 {
                                                                                                                                                                     id = u.UserId, text = u.EmailAddress
                                                                                                                                                                 }) // normalize usersNotInGroup for Kendo Tree
                .ToArray();

            var serializer = new JavaScriptSerializer();
            string ret = serializer.Serialize(returnKendo); // add kendo tree

            if (usersNotInGroup.Length > 0)
            {
                ret += serializer.Serialize(usersNotInGroup); // append users not in group
                ret = ret.Replace("][", ","); // merge the 2 serialized arrays
            }

            return ret;
        }

        /// <summary>
        /// GET: api/GetUserGroupsAndUsers
        /// </summary>
        /// Client ID
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<string> GetUserGroupsAndUsers()
        {
            var userGroupTree = new List<UserGroupNode>();
            var userIdList = new List<int>();

            IQueryable<UserGroup> groups = this.db.UserGroups.Where(g => g.ClientId == this.ClientId);

            foreach (UserGroup grp in groups)
            {
                IQueryable<User> query = from usersInUserGroups in this.db.UsersInUserGroups join user in this.db.Users on usersInUserGroups.UserId equals user.UserId where usersInUserGroups.UserGroupId == grp.UserGroupId && user.ClientId == this.ClientId select user;

                userGroupTree.Add(new UserGroupNode
                                      {
                                          UserGroup = grp, Users = query.ToArray()
                                      });

                userIdList.AddRange(query.Select(x => x.UserId));
            }

            // normalize userGroupTree for Kendo Tree
            var returnKendo = userGroupTree.Select(g => new
                                                            {
                                                                id = g.UserGroup.UserGroupId, text = g.UserGroup.Name, expanded = true, spriteCssClass = "group", items = g.Users.Select(u => new
                                                                                                                                                                                                  {
                                                                                                                                                                                                      id = u.UserId, text = u.FirstName + " " + u.LastName
                                                                                                                                                                                                  }).ToArray()
                                                            });

            // grab users for this client, who are not in a group and add them to the end of the tree
            var usersNotInGroup = this.db.Users.Where(x => x.ClientId == this.ClientId).ToArray().Where(x => userIdList.IndexOf(x.UserId) == -1).Select(u => new
                                                                                                                                                                 {
                                                                                                                                                                     id = u.UserId, text = u.FirstName + " " + u.LastName
                                                                                                                                                                 }) // normalize usersNotInGroup for Kendo Tree
                .ToArray();

            var serializer = new JavaScriptSerializer();
            string ret = serializer.Serialize(returnKendo); // add kendo tree

            if (usersNotInGroup.Length > 0)
            {
                ret += serializer.Serialize(usersNotInGroup); // append users not in group
                ret = ret.Replace("][", ","); // merge the 2 serialized arrays
            }

            return ret;
        }

        // PUT: api/UsersInGroups/5
        /// <summary>
        /// The update users in group.
        /// </summary>
        /// <param name="groups">
        /// The groups.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        public async Task<IHttpActionResult> UpdateUsersInGroup([FromBody] UsersInUserGroup[] groups)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            foreach (UsersInUserGroup group in groups)
            {
                // remove items which are already grouped
                if (this.db.UsersInUserGroups.Any(u => u.UserId == group.UserId))
                {
                    UsersInUserGroup record = this.db.UsersInUserGroups.Single(u => u.UserId == group.UserId);
                    this.db.UsersInUserGroups.Remove(record);
                }

                this.db.UsersInUserGroups.Add(group);

                try
                {
                    this.db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }

            return this.Ok(groups);
        }

        // POST: api/UsersInGroups
        // [HttpPost]
        // public IHttpActionResult AddUsersToGroup(User[] users)
        // {
        // if (!ModelState.IsValid)
        // {
        // return BadRequest(ModelState);
        // }

        // foreach (User user in users)
        // {
        // db.Users.Add(user);
        // db.SaveChanges();
        // }

        // return Ok(users);
        // }

        // DELETE: api/UsersInGroups/5
        // [HttpDelete]
        // public IHttpActionResult DeleteUsersInGroup(int id)
        // {
        // UsersInUserGroup usersInGroup = db.UsersInUserGroups.Find(id);
        // if (usersInGroup == null)
        // {
        // return NotFound();
        // }

        // db.UsersInUserGroups.Remove(usersInGroup);
        // db.SaveChanges();

        // return Ok(usersInGroup);
        // }

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
    }
}