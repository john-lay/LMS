using LMS.API.Contexts;
using LMS.API.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LMS.API.Controllers
{
    public class UserGroupsController : ApiBaseController
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
            group.ClientId = this.ClientId;

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
        public IHttpActionResult CreateGroup(UserGroup group)
        {
            group.ClientId = this.ClientId;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //generate group id
            group.UserGroupId = db.UserGroups.Count();

            //set parent id to root. i.e. -1
            group.ParentId = -1;

            db.UserGroups.Add(group);
            db.SaveChanges();

            return Ok(group);
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