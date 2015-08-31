// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CoursesController.cs" company="">
//   
// </copyright>
// <summary>
//   The courses controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Controllers
{
    using System.Collections.Generic;
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
    /// The courses controller.
    /// </summary>
    public class CoursesController : ApiBaseController
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LMSContext db = new LMSContext();

        /// <summary>
        /// The create course.
        /// </summary>
        /// <param name="course">
        /// The course.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        public async Task<IHttpActionResult> CreateCourse(Course course)
        {
            course.ClientId = this.ClientId;

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.db.Courses.Add(course);
            this.db.SaveChanges();

            return this.Ok(course);
        }

        /// <summary>
        /// The delete course.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCourse(int id)
        {
            Course course = this.db.Courses.Find(id);
            if (course == null)
            {
                return this.NotFound();
            }

            this.db.Courses.Remove(course);
            this.db.SaveChanges();

            return this.Ok(course);
        }

        /// <summary>
        /// The get course.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetCourse(int id)
        {
            Course course = this.db.Courses.Find(id);
            if (course == null)
            {
                return this.NotFound();
            }

            return this.Ok(course);
        }

        /// <summary>
        /// The get courses by client.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetCoursesByClient(int id)
        {
            IEnumerable<Course> courses = this.db.Courses
                .Where(c => c.ClientId == id)
                .Select(c => new
                {
                    c.CourseId, 
                    c.Name, 
                    c.Description
                })
                .ToList()
                .Select(c => new Course
                {
                    CourseId = c.CourseId, 
                    Name = c.Name, 
                    Description = c.Description
                });

            return this.Ok(courses);
        }

        /// <summary>
        /// The get courses by user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        public async Task<string> GetCoursesByUser(int id)
        {
            // courses, session, content
            var query = (from usersInCourseSession in this.db.UsersInCourseSessions
                         join courseSession in this.db.CourseSessions on usersInCourseSession.CourseSessionId equals courseSession.CourseSessionId
                         join course in this.db.Courses on courseSession.CourseId equals course.CourseId
                         join content in this.db.Contents on course.CourseId equals content.CourseId
                         where usersInCourseSession.UserId == id
                         select new
                        {
                            CourseName = course.Name, 
                            CourseDescription = course.Description, 
                            CourseContentName = content.Name, 
                            CourseContentDescription = content.Description, 
                            CourseContentResource = content.Resource, 
                            courseSession.CourseSessionId, 
                            CourseSessionStartDate = courseSession.StartDate, 
                            CourseSessionEndDate = courseSession.EndDate, 
                            usersInCourseSession.LearningComplete
                        })
                        .Distinct()
                        .ToArray();

            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(query);
        }

        //// PUT: api/Courses/5
        // [HttpPost]
        // public HttpResponseMessage UpdateCourse(int id)
        // {
        // var identity = Thread.CurrentPrincipal.Identity;

        // HttpResponseMessage result = null;
        // var httpRequest = HttpContext.Current.Request;

        // if (httpRequest.Files.Count > 0)
        // {
        // var docfiles = new List<string>();
        // foreach (string file in httpRequest.Files)
        // {
        // var postedFile = httpRequest.Files[file];
        // var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
        // postedFile.SaveAs(filePath);

        // docfiles.Add(filePath);
        // }
        // result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
        // }
        // else
        // {
        // result = Request.CreateResponse(HttpStatusCode.BadRequest);
        // }

        // //httpRequest.Form["Name"]

        // //db.Clients.Add(client);
        // //db.SaveChanges();

        // return result;
        // }

        /// <summary>
        /// The update course.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="course">
        /// The course.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        public async Task<IHttpActionResult> UpdateCourse(int id, Course course)
        {
            course.ClientId = this.ClientId;

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.db.Entry(course).State = EntityState.Modified;

            try
            {
                this.db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.CourseExists(id))
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
        /// The course exists.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CourseExists(int id)
        {
            return this.db.Courses.Count(e => e.CourseId == id) > 0;
        }
    }
}