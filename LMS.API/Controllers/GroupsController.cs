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
    public class GroupsController : ApiController
    {
        private LMSContext db = new LMSContext();

        // GET: api/Groups
        public IQueryable<Group> GetGroups()
        {
            return db.Groups;
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
            var ret = db.Groups.Find(id);
            return ret == null ? Request.CreateErrorResponse(HttpStatusCode.NotFound, "group not found")
                               : Request.CreateResponse(HttpStatusCode.OK, ret);
        }
        
        // PUT: api/Groups/5
        [HttpPut]
        public IHttpActionResult UpdateGroup(int id, Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != group.GroupId)
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
        public HttpResponseMessage CreateGroup(Group group)
        {
            HttpResponseMessage msg;

            if (!ModelState.IsValid)
            {
                msg = Request.CreateResponse(HttpStatusCode.NotFound, "Error creating group");
                return msg;
            }

            //generate group id
            group.GroupId = db.Groups.Count();

            db.Groups.Add(group);
            db.SaveChanges();

            msg = Request.CreateResponse(HttpStatusCode.Created);
            msg.Headers.Location = new Uri(Request.RequestUri + group.GroupId.ToString());

            return msg;
        }

        // DELETE: api/Groups/5
        [HttpDelete]
        public IHttpActionResult DeleteGroup(int id)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return NotFound();
            }

            db.Groups.Remove(group);
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
            return db.Groups.Count(e => e.GroupId == id) > 0;
        }
    }
}