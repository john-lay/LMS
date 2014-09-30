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
using LMS.API.Contexts;

namespace LMS.API.Controllers
{
    public class UserGroupsController : ApiController
    {
        private LMSContext db = new LMSContext();

        // GET: api/Groups
        public IQueryable<UserGroup> GetGroups()
        {
            return db.UserGroups;
        }

        // GET: api/Groups/5
        [HttpGet]
        public HttpResponseMessage GetGroup(int id)
        {
            //Group group = db.Groups.Find(id);
            //if (group == null)
            //{
            //    return NotFound();
            //}

            //return Ok(group);
            var ret = db.UserGroups.Find(id);
            return ret == null ? Request.CreateErrorResponse(HttpStatusCode.NotFound, "group not found")
                               : Request.CreateResponse(HttpStatusCode.OK, ret);
        }
        
        // PUT: api/Groups/5
        [HttpPut]
        public IHttpActionResult UpdateGroup(int id, UserGroup group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != group.UserGroupId)
            {
                return BadRequest();
            }

            db.Entry(group).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
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

        // POST: api/Groups
        [HttpPost]
        public HttpResponseMessage CreateGroup(UserGroup group)
        {
            HttpResponseMessage msg;

            if (!ModelState.IsValid)
            {
                msg = Request.CreateResponse(HttpStatusCode.NotFound, "Error creating group");
                return msg;
            }

            //generate group id
            group.UserGroupId = db.UserGroups.Count();

            db.UserGroups.Add(group);
            db.SaveChanges();

            msg = Request.CreateResponse(HttpStatusCode.Created);
            msg.Headers.Location = new Uri(Request.RequestUri + group.UserGroupId.ToString());

            return msg;
        }

        // DELETE: api/Groups/5
        [HttpDelete]
        public IHttpActionResult DeleteGroup(int id)
        {
            UserGroup group = db.UserGroups.Find(id);
            if (group == null)
            {
                return NotFound();
            }

            db.UserGroups.Remove(group);
            db.SaveChanges();

            return Ok(group);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GroupExists(int id)
        {
            return db.UserGroups.Count(e => e.UserGroupId == id) > 0;
        }
    }
}