// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsersInCourseSessionsController.cs" company="">
//   
// </copyright>
// <summary>
//   The users in course sessions controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Controllers
{
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Script.Serialization;

    using LMS.API.Contexts;
    using LMS.API.Models;

    /// <summary>
    /// The users in course sessions controller.
    /// </summary>
    public class UsersInCourseSessionsController : ApiBaseController
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LMSContext db = new LMSContext();

        /// <summary>
        /// The add users to course session.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="users">
        /// The users.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        public async Task<IHttpActionResult> AddUsersToCourseSession(int id, [FromBody] User[] users)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            foreach (User user in users)
            {
                // check user isn't already registered in this session
                if (this.UsersInCourseSessionExists(id, user.UserId))
                {
                    return this.BadRequest(this.ModelState);
                }

                this.db.UsersInCourseSessions.Add(new UsersInCourseSession
                                                      {
                                                          CourseSessionId = id, UserId = user.UserId
                                                      });
                this.db.SaveChanges();
            }

            return this.Ok();
        }

        /// <summary>
        /// The get users in course session.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        public async Task<string> GetUsersInCourseSession(int id)
        {
            UsersInCourseSession[] usersInCourseSession = this.db.UsersInCourseSessions.Where(x => x.CourseSessionId == id).ToArray();
            if (usersInCourseSession == null)
            {
                return "Error: No users in session";
            }

            var users = usersInCourseSession.Select(u => new
                                                             {
                                                                 u.UserId, Name = u.User.FirstName + " " + u.User.LastName
                                                             }).ToArray();

            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(users);
        }

        // POST: api/RemoveUsersFromCourseSession
        /// <summary>
        /// The remove users from course session.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="users">
        /// The users.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        public async Task<IHttpActionResult> RemoveUsersFromCourseSession(int id, [FromBody] User[] users)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            foreach (User user in users)
            {
                if (!this.UsersInCourseSessionExists(id, user.UserId))
                {
                    return this.BadRequest(this.ModelState);
                }

                var entityToRemove = new UsersInCourseSession
                                         {
                                             CourseSessionId = id, UserId = user.UserId
                                         };
                this.db.UsersInCourseSessions.Attach(entityToRemove);
                this.db.UsersInCourseSessions.Remove(entityToRemove);
                this.db.SaveChanges();
            }

            return this.Ok();
        }

        /// <summary>
        /// The update user result in course session.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="userInCourseSession">
        /// The user in course session.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPatch]
        public async Task<IHttpActionResult> UpdateUserResultInCourseSession(int id, [FromBody] UsersInCourseSession userInCourseSession)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != userInCourseSession.CourseSessionId)
            {
                return this.BadRequest();
            }

            // grab the user details from a existing record. Don't want to overwrite AspNetUserId or email
            UsersInCourseSession existingUser = this.db.UsersInCourseSessions.First(u => u.CourseSessionId == userInCourseSession.CourseSessionId && u.UserId == userInCourseSession.UserId);

            existingUser.LearningComplete = userInCourseSession.LearningComplete;

            try
            {
                this.db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.UsersInCourseSessionExists(id, userInCourseSession.UserId))
                {
                    return this.NotFound();
                }

                throw;
            }

            return this.StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// The users in course session exists.
        /// </summary>
        /// <param name="courseSessionId">
        /// The course session id.
        /// </param>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool UsersInCourseSessionExists(int courseSessionId, int userId)
        {
            return this.db.UsersInCourseSessions.Count(e => e.CourseSessionId == courseSessionId && e.UserId == userId) > 0;
        }
    }
}