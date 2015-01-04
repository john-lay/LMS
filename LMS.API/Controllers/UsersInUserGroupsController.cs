using LMS.API.Contexts;
using LMS.API.Models;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace LMS.API.Controllers
{
    public class UsersInUserGroupsController : ApiBaseController
    {
        private LMSContext db = new LMSContext();

        // GET: api/UsersInGroups
        //public IQueryable<UsersInUserGroup> GetGroupUser()
        //{
        //    return db.UsersInUserGroups;
        //}

        /// <summary>
        /// GET: api/GetUserGroupsAndUsers
        /// </summary>
        /// <param name="ijohnd">Client ID</param>
        /// <returns></returns>
        public string GetUserGroupsAndUsers()
        {
            List<UserGroupNode> userGroupTree = new List<UserGroupNode>();
            List<int> userIdList = new List<int>();

            var groups = db.UserGroups;

            foreach (var grp in groups)
            {
                var query = from usersInUserGroups in db.UsersInUserGroups
                            join user in db.Users on usersInUserGroups.UserId equals user.UserId
                            where usersInUserGroups.UserGroupId == grp.UserGroupId && user.ClientId == this.ClientId
                            select user;

                userGroupTree.Add(new UserGroupNode
                {
                    UserGroup = grp,
                    Users = query.ToArray()
                });

                userIdList.AddRange(query.Select(x => x.UserId));
            }

            // normalize userGroupTree for Kendo Tree
            var returnKendo = userGroupTree
                .Select(g => new
                {
                    id = g.UserGroup.UserGroupId,
                    text = g.UserGroup.Name,
                    expanded = true,
                    spriteCssClass = "group",
                    items = g.Users.Select(u => new { id = u.UserId, text = u.FirstName + " " + u.LastName }).ToArray()
                });

            // grab users for this client, who are not in a group and add them to the end of the tree
            var usersNotInGroup = db.Users
                .Where(x => x.ClientId == this.ClientId)
                .ToArray()
                .Where(x => userIdList.IndexOf(x.UserId) == -1)
                .Select(u => new { id = u.UserId, text = u.FirstName + " " + u.LastName })  // normalize usersNotInGroup for Kendo Tree
                .ToArray();
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string ret = serializer.Serialize(returnKendo); // add kendo tree

            if (usersNotInGroup.Length > 0)
            {
                ret += serializer.Serialize(usersNotInGroup); // append users not in group
                ret = ret.Replace("][", ","); // merge the 2 serialized arrays
            }

            return ret;
        }

        public string GetUserGroupsAndUserEmails()
        {
            List<UserGroupNode> userGroupTree = new List<UserGroupNode>();
            List<int> userIdList = new List<int>();

            var groups = db.UserGroups;

            foreach (var grp in groups)
            {
                var query = from usersInUserGroups in db.UsersInUserGroups
                            join user in db.Users on usersInUserGroups.UserId equals user.UserId
                            where usersInUserGroups.UserGroupId == grp.UserGroupId && user.ClientId == this.ClientId
                            select user;

                userGroupTree.Add(new UserGroupNode
                {
                    UserGroup = grp,
                    Users = query.ToArray()
                });

                userIdList.AddRange(query.Select(x => x.UserId));
            }

            // normalize userGroupTree for Kendo Tree
            var returnKendo = userGroupTree
                .Select(g => new
                {
                    id = g.UserGroup.UserGroupId,
                    text = g.UserGroup.Name,
                    expanded = true,
                    spriteCssClass = "group",
                    items = g.Users.Select(u => new { id = u.UserId, text = u.EmailAddress }).ToArray()
                });

            // grab users for this client, who are not in a group and add them to the end of the tree
            var usersNotInGroup = db.Users
                .Where(x => x.ClientId == this.ClientId)
                .ToArray()
                .Where(x => userIdList.IndexOf(x.UserId) == -1)
                .Select(u => new { id = u.UserId, text = u.EmailAddress })  // normalize usersNotInGroup for Kendo Tree
                .ToArray();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string ret = serializer.Serialize(returnKendo); // add kendo tree

            if (usersNotInGroup.Length > 0)
            {
                ret += serializer.Serialize(usersNotInGroup); // append users not in group
                ret = ret.Replace("][", ","); // merge the 2 serialized arrays
            }

            return ret;
        }

        // PUT: api/UsersInGroups/5
        [HttpPut]
        public IHttpActionResult UpdateUsersInGroup([FromBody]UsersInUserGroup[] groups)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (UsersInUserGroup group in groups)
            {
                // remove items which are already grouped
                if (db.UsersInUserGroups.Any(u => u.UserId == group.UserId))
                {
                    UsersInUserGroup record = db.UsersInUserGroups.Single(u => u.UserId == group.UserId);
                    db.UsersInUserGroups.Remove(record);
                }

                db.UsersInUserGroups.Add(group);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            return Ok(groups);
        }

        // POST: api/UsersInGroups
        //[HttpPost]
        //public IHttpActionResult AddUsersToGroup(User[] users)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    foreach (User user in users)
        //    {
        //        db.Users.Add(user);
        //        db.SaveChanges();
        //    }

        //    return Ok(users);
        //}

        // DELETE: api/UsersInGroups/5
        //[HttpDelete]
        //public IHttpActionResult DeleteUsersInGroup(int id)
        //{
        //    UsersInUserGroup usersInGroup = db.UsersInUserGroups.Find(id);
        //    if (usersInGroup == null)
        //    {
        //        return NotFound();
        //    }

        //    db.UsersInUserGroups.Remove(usersInGroup);
        //    db.SaveChanges();

        //    return Ok(usersInGroup);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}