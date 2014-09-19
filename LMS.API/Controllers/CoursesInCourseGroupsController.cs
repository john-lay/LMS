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
    public class CoursesInCourseGroupsController : ApiController
    {
        private LMSContext db = new LMSContext();

        // GET: api/CoursesInCourseGroups
        public IQueryable<CoursesInCourseGroup> GetCoursesInCourseGroups()
        {
            return db.CoursesInCourseGroups;
        }

        // GET: api/CoursesInCourseGroups/5
        [ResponseType(typeof(CoursesInCourseGroup))]
        public IHttpActionResult GetCoursesInCourseGroup(int id)
        {
            CoursesInCourseGroup coursesInCourseGroup = db.CoursesInCourseGroups.Find(id);
            if (coursesInCourseGroup == null)
            {
                return NotFound();
            }

            return Ok(coursesInCourseGroup);
        }

        // PUT: api/CoursesInCourseGroups/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCoursesInCourseGroup(int id, CoursesInCourseGroup coursesInCourseGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != coursesInCourseGroup.CourseGroupId)
            {
                return BadRequest();
            }

            db.Entry(coursesInCourseGroup).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoursesInCourseGroupExists(id))
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

        // POST: api/CoursesInCourseGroups
        [ResponseType(typeof(CoursesInCourseGroup))]
        public IHttpActionResult PostCoursesInCourseGroup(CoursesInCourseGroup coursesInCourseGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CoursesInCourseGroups.Add(coursesInCourseGroup);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CoursesInCourseGroupExists(coursesInCourseGroup.CourseGroupId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = coursesInCourseGroup.CourseGroupId }, coursesInCourseGroup);
        }

        // DELETE: api/CoursesInCourseGroups/5
        [ResponseType(typeof(CoursesInCourseGroup))]
        public IHttpActionResult DeleteCoursesInCourseGroup(int id)
        {
            CoursesInCourseGroup coursesInCourseGroup = db.CoursesInCourseGroups.Find(id);
            if (coursesInCourseGroup == null)
            {
                return NotFound();
            }

            db.CoursesInCourseGroups.Remove(coursesInCourseGroup);
            db.SaveChanges();

            return Ok(coursesInCourseGroup);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CoursesInCourseGroupExists(int id)
        {
            return db.CoursesInCourseGroups.Count(e => e.CourseGroupId == id) > 0;
        }
    }
}