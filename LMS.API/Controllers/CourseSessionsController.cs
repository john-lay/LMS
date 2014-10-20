using LMS.API.Contexts;
using LMS.API.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace LMS.API.Controllers
{
    public class CourseSessionsController : ApiBaseController
    {
        private LMSContext db = new LMSContext();

        // GET: api/CourseSessions
        [HttpGet]
        public IQueryable<CourseSession> GetCourseSessions(int id)
        {
            return db.CourseSessions.Where(s => s.CourseId == id);
        }

        // GET: api/CourseSessions/5
        //[ResponseType(typeof(CourseSession))]
        //public IHttpActionResult GetCourseSession(int id)
        //{
        //    CourseSession courseSession = db.CourseSessions.Find(id);
        //    if (courseSession == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(courseSession);
        //}

        // PUT: api/CourseSessions/5
        [HttpPut]
        public IHttpActionResult UpdateCourseSession(int id, CourseSession courseSession)
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
        [HttpPost]
        public IHttpActionResult CreateCourseSession(CourseSession courseSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CourseSessions.Add(courseSession);
            db.SaveChanges();

            return Ok(courseSession);
        }

        // DELETE: api/CourseSessions/5
        [HttpDelete]
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