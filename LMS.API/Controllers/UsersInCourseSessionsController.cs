using LMS.API.Contexts;
using LMS.API.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;

namespace LMS.API.Controllers
{
    public class UsersInCourseSessionsController : ApiBaseController
    {
        private LMSContext db = new LMSContext();

        // GET: api/UsersInCourseSessions/5
        [HttpGet]
        public string GetUsersInCourseSession(int id)
        {
            UsersInCourseSession[] usersInCourseSession = db.UsersInCourseSessions.Where(x => x.CourseSessionId == id).ToArray();
            if (usersInCourseSession == null)
            {
                return "Error: No users in session";
            }

            var users = usersInCourseSession.Select(u => new 
            {
                UserId = u.UserId,
                Name = u.User.FirstName + " " + u.User.LastName
            }).ToArray();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(users);
        }

        // POST: api/AddUsersToCourseSession
        [HttpPost]
        public IHttpActionResult AddUsersToCourseSession(int id, [FromBody]User[] users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (User user in users)
            {
                // check user isn't already registered in this session
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

        // POST: api/RemoveUsersFromCourseSession
        [HttpPost]
        public IHttpActionResult RemoveUsersFromCourseSession(int id, [FromBody]User[] users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (User user in users)
            {
                if (!UsersInCourseSessionExists(id, user.UserId))
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    var entityToRemove = new UsersInCourseSession { CourseSessionId = id, UserId = user.UserId };
                    db.UsersInCourseSessions.Attach(entityToRemove);
                    db.UsersInCourseSessions.Remove(entityToRemove);
                    db.SaveChanges();
                }
            }

            return Ok();
        }

        [HttpPatch]
        public IHttpActionResult UpdateUserResultInCourseSession(int id, [FromBody] UsersInCourseSession userInCourseSession)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userInCourseSession.CourseSessionId)
            {
                return BadRequest();
            }

            // grab the user details from a existing record. Don't want to overwrite AspNetUserId or email
            UsersInCourseSession existingUser = db.UsersInCourseSessions.First(u => u.CourseSessionId == userInCourseSession.CourseSessionId && u.UserId == userInCourseSession.UserId);

            existingUser.LearningComplete = userInCourseSession.LearningComplete;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsersInCourseSessionExists(id, userInCourseSession.UserId))
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