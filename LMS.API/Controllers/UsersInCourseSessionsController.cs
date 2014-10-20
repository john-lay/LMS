using LMS.API.Contexts;
using LMS.API.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;

namespace LMS.API.Controllers
{
    public class UsersInCourseSessionsController : ApiBaseController
    {
        private LMSContext db = new LMSContext();

        // GET: api/UsersInCourseSessions
        public IQueryable<UsersInCourseSession> GetUsersInCourseSessions()
        {
            return db.UsersInCourseSessions;
        }

        // GET: api/UsersInCourseSessions/5
        [ResponseType(typeof(UsersInCourseSession))]
        public IHttpActionResult GetUsersInCourseSession(int id)
        {
            UsersInCourseSession usersInCourseSession = db.UsersInCourseSessions.Find(id);
            if (usersInCourseSession == null)
            {
                return NotFound();
            }

            return Ok(usersInCourseSession);
        }

        // PUT: api/UsersInCourseSessions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsersInCourseSession(int id, UsersInCourseSession usersInCourseSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usersInCourseSession.CourseSessionId)
            {
                return BadRequest();
            }

            db.Entry(usersInCourseSession).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersInCourseSessionExists(id))
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

        // POST: api/UsersInCourseSessions
        [ResponseType(typeof(UsersInCourseSession))]
        public IHttpActionResult PostUsersInCourseSession(UsersInCourseSession usersInCourseSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UsersInCourseSessions.Add(usersInCourseSession);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UsersInCourseSessionExists(usersInCourseSession.CourseSessionId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = usersInCourseSession.CourseSessionId }, usersInCourseSession);
        }

        // DELETE: api/UsersInCourseSessions/5
        [ResponseType(typeof(UsersInCourseSession))]
        public IHttpActionResult DeleteUsersInCourseSession(int id)
        {
            UsersInCourseSession usersInCourseSession = db.UsersInCourseSessions.Find(id);
            if (usersInCourseSession == null)
            {
                return NotFound();
            }

            db.UsersInCourseSessions.Remove(usersInCourseSession);
            db.SaveChanges();

            return Ok(usersInCourseSession);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsersInCourseSessionExists(int id)
        {
            return db.UsersInCourseSessions.Count(e => e.CourseSessionId == id) > 0;
        }
    }
}