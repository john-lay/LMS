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
    public class CourseGroupsController : ApiController
    {
        private LMSContext db = new LMSContext();

        // GET: api/CourseGroups
        public IQueryable<CourseGroup> GetCourseGroups()
        {
            return db.CourseGroups;
        }

        // GET: api/CourseGroups/5
        [ResponseType(typeof(CourseGroup))]
        public IHttpActionResult GetCourseGroup(int id)
        {
            CourseGroup courseGroup = db.CourseGroups.Find(id);
            if (courseGroup == null)
            {
                return NotFound();
            }

            return Ok(courseGroup);
        }

        // PUT: api/CourseGroups/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCourseGroup(int id, CourseGroup courseGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != courseGroup.CourseGroupId)
            {
                return BadRequest();
            }

            db.Entry(courseGroup).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseGroupExists(id))
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

        // POST: api/CourseGroups
        [ResponseType(typeof(CourseGroup))]
        public IHttpActionResult PostCourseGroup(CourseGroup courseGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CourseGroups.Add(courseGroup);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = courseGroup.CourseGroupId }, courseGroup);
        }

        // DELETE: api/CourseGroups/5
        [ResponseType(typeof(CourseGroup))]
        public IHttpActionResult DeleteCourseGroup(int id)
        {
            CourseGroup courseGroup = db.CourseGroups.Find(id);
            if (courseGroup == null)
            {
                return NotFound();
            }

            db.CourseGroups.Remove(courseGroup);
            db.SaveChanges();

            return Ok(courseGroup);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseGroupExists(int id)
        {
            return db.CourseGroups.Count(e => e.CourseGroupId == id) > 0;
        }
    }
}