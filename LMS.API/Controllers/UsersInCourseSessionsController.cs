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
        [HttpPut]
        public IHttpActionResult UpdateUsersInCourseSession(int id, UsersInCourseSession usersInCourseSession)
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
                //if (!UsersInCourseSessionExists(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/UsersInCourseSessions
        [HttpPost]
        public IHttpActionResult AddUsersToCourseSession(int id, [FromBody]User[] users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (User user in users)
            {
                // prevent the creation of users with an already registered email address
                if (UsersInCourseSessionExists(id, user.UserId))
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    db.UsersInCourseSessions.Add(new UsersInCourseSession { CourseSessionId = id, UserId = user.UserId});
                    db.SaveChanges();
                }
            }

            return Ok();
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

        private bool UsersInCourseSessionExists(int courseSessionId, int userId)
        {
            return db.UsersInCourseSessions.Count(e => e.CourseSessionId == courseSessionId && e.UserId == userId) > 0;
        }
    }
}