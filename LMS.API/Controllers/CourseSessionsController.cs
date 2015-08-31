// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CourseSessionsController.cs" company="">
//   
// </copyright>
// <summary>
//   The course sessions controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Controllers
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Script.Serialization;

    using LMS.API.Contexts;
    using LMS.API.Models;

    /// <summary>
    /// The course sessions controller.
    /// </summary>
    public class CourseSessionsController : ApiBaseController
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LMSContext db = new LMSContext();

        //// GET: api/CourseSessions
        // [HttpGet]
        // public IQueryable<CourseSession> GetCourseSessions(int id)
        // {
        // return db.CourseSessions.Where(s => s.CourseId == id);
        // }

        /// <summary>
        /// The create course session.
        /// </summary>
        /// <param name="courseSession">
        /// The course session.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        public async Task<IHttpActionResult> CreateCourseSession(CourseSession courseSession)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.db.CourseSessions.Add(courseSession);
            this.db.SaveChanges();

            return this.Ok(courseSession);
        }

        /// <summary>
        /// The delete course session.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCourseSession(int id)
        {
            CourseSession courseSession = this.db.CourseSessions.Find(id);
            if (courseSession == null)
            {
                return this.NotFound();
            }

            this.db.CourseSessions.Remove(courseSession);
            this.db.SaveChanges();

            return this.Ok(courseSession);
        }

        /// <summary>
        /// The get course sessions.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        public async Task<string> GetCourseSessions(int id)
        {
            var serializer = new JavaScriptSerializer();
            CourseSession[] query = this.db.CourseSessions.Where(s => s.CourseId == id).ToArray();
            return serializer.Serialize(query);
        }

        /// <summary>
        /// The update course session.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="courseSession">
        /// The course session.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        public async Task<IHttpActionResult> UpdateCourseSession(int id, CourseSession courseSession)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != courseSession.CourseSessionId)
            {
                return this.BadRequest();
            }

            this.db.Entry(courseSession).State = EntityState.Modified;

            try
            {
                this.db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.CourseSessionExists(id))
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
        /// The course session exists.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CourseSessionExists(int id)
        {
            return this.db.CourseSessions.Count(e => e.CourseSessionId == id) > 0;
        }
    }
}