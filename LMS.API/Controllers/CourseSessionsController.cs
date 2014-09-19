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
    public class CourseSessionsController : ApiController
    {
        private LMSContext db = new LMSContext();

        // GET: api/CourseSessions
        public IQueryable<CourseSession> GetCourseSessions()
        {
            return db.CourseSessions;
        }

        // GET: api/CourseSessions/5
        [ResponseType(typeof(CourseSession))]
        public IHttpActionResult GetCourseSession(int id)
        {
            CourseSession courseSession = db.CourseSessions.Find(id);
            if (courseSession == null)
            {
                return NotFound();
            }

            return Ok(courseSession);
        }

        // PUT: api/CourseSessions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCourseSession(int id, CourseSession courseSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != courseSession.CourseSessionId)
            {
                return BadRequest();
            }

            db.Entry(courseSession).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseSessionExists(id))
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

        // POST: api/CourseSessions
        [ResponseType(typeof(CourseSession))]
        public IHttpActionResult PostCourseSession(CourseSession courseSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CourseSessions.Add(courseSession);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = courseSession.CourseSessionId }, courseSession);
        }

        // DELETE: api/CourseSessions/5
        [ResponseType(typeof(CourseSession))]
        public IHttpActionResult DeleteCourseSession(int id)
        {
            CourseSession courseSession = db.CourseSessions.Find(id);
            if (courseSession == null)
            {
                return NotFound();
            }

            db.CourseSessions.Remove(courseSession);
            db.SaveChanges();

            return Ok(courseSession);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseSessionExists(int id)
        {
            return db.CourseSessions.Count(e => e.CourseSessionId == id) > 0;
        }
    }
}