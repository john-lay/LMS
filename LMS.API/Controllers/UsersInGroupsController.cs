using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using LMS.API.Models;
using LMS.Services.Contexts;

namespace LMS.API.Controllers
{
    public class UsersInGroupsController : ApiController
    {
        private LMSContext db = new LMSContext();

        // GET: api/UsersInGroups
        public IQueryable<UsersInGroup> GetGroupUser()
        {
            return db.GroupUser;
        }

        // GET: api/UsersInGroups/5
        [HttpGet]
        public IHttpActionResult GetUsersInGroup(int id)
        {
            UsersInGroup usersInGroup = db.GroupUser.Find(id);
            if (usersInGroup == null)
            {
                return NotFound();
            }

            return Ok(usersInGroup);
        }

        // PUT: api/UsersInGroups/5
        [HttpPut]
        public HttpResponseMessage UpdateUsersInGroup(int id, [FromBody]User[] users)
        {
            HttpResponseMessage msg;

            if (!ModelState.IsValid)
            {
                msg = Request.CreateResponse(HttpStatusCode.NotFound, "Error adding users to group");
                return msg;
            }

            foreach (User user in users)
            {
                db.GroupUser.Add(new UsersInGroup { GroupId = id, UserId = user.UserId });
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!UsersInGroupExists(id))
                //{
                //    msg = Request.CreateResponse(HttpStatusCode.NotFound, "Error adding users to group");
                //}
                //else
                //{
                //    throw;
                //}
            }

            msg = Request.CreateResponse(HttpStatusCode.Created);
            //msg.Headers.Location = new Uri(Request.RequestUri + group.GroupId.ToString());

            return msg;
        }

        // POST: api/UsersInGroups
        [HttpPost]
        public IHttpActionResult AddUsersToGroup([FromBody]User[] users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (User user in users)
            {
                db.Users.Add(user);
                db.SaveChanges();
            }

            return Ok(users);
        }

        // DELETE: api/UsersInGroups/5
        [HttpDelete]
        public IHttpActionResult DeleteUsersInGroup(int id)
        {
            UsersInGroup usersInGroup = db.GroupUser.Find(id);
            if (usersInGroup == null)
            {
                return NotFound();
            }

            db.GroupUser.Remove(usersInGroup);
            db.SaveChanges();

            return Ok(usersInGroup);
        }

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