using LMS.API.Contexts;
using LMS.API.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace LMS.API.Controllers
{
    public class CoursesController : ApiBaseController
    {
        private LMSContext db = new LMSContext();

        // GET: api/Users/5
        [HttpGet]
        public IHttpActionResult GetCourse(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        [HttpGet]
        public IHttpActionResult GetCoursesByClient(int id)
        {
            IEnumerable<Course> courses = db.Courses
                .Where(c => c.ClientId == id)
                .Select(c => new
                {
                    CourseId = c.CourseId,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToList()
                .Select(c => new Course
                {
                    CourseId = c.CourseId,
                    Name = c.Name,
                    Description = c.Description
                });

            return Ok(courses);
        }

        [HttpGet]
        public string GetCoursesByUser(int id)
        { 
            // courses, session, content
            var query = (from usersInCourseSession in db.UsersInCourseSessions
                        join courseSession in db.CourseSessions on usersInCourseSession.CourseSessionId equals courseSession.CourseSessionId
                        join course in db.Courses on courseSession.CourseId equals course.CourseId
                        join content in db.Contents on course.CourseId equals content.CourseId
                        where usersInCourseSession.UserId == id
                        select new 
                        {
                            CourseName = course.Name,
                            CourseDescription = course.Description,
                            CourseContentName = content.Name,
                            CourseContentDescription = content.Description,
                            CourseContentResource = content.Resource,
                            CourseSessionId = courseSession.CourseSessionId,
                            CourseSessionStartDate = courseSession.StartDate,
                            CourseSessionEndDate = courseSession.EndDate,
                            LearningComplete = usersInCourseSession.LearningComplete
                        })
                        .Distinct()
                        .ToArray();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(query);
        }

        //// PUT: api/Courses/5
        //[HttpPost]
        //public HttpResponseMessage UpdateCourse(int id)
        //{
        //    var identity = Thread.CurrentPrincipal.Identity;

        //    HttpResponseMessage result = null;
        //    var httpRequest = HttpContext.Current.Request;

        //    if (httpRequest.Files.Count > 0)
        //    {
        //        var docfiles = new List<string>();
        //        foreach (string file in httpRequest.Files)
        //        {
        //            var postedFile = httpRequest.Files[file];
        //            var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
        //            postedFile.SaveAs(filePath);

        //            docfiles.Add(filePath);
        //        }
        //        result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
        //    }
        //    else
        //    {
        //        result = Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }

        //    //httpRequest.Form["Name"]

        //    //db.Clients.Add(client);
        //    //db.SaveChanges();

        //    return result;
        //}
        [HttpPut]
        public IHttpActionResult UpdateCourse(int id, Course course)
        {
            course.ClientId = this.ClientId;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(course).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/Courses
        [HttpPost]
        public IHttpActionResult CreateCourse(Course course)
        {
            course.ClientId = this.ClientId;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Courses.Add(course);
            db.SaveChanges();

            return Ok(course);
        }

        // DELETE: api/Courses/5
        [HttpDelete]
        public IHttpActionResult DeleteCourse(int id)
        {
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            db.Courses.Remove(course);
            db.SaveChanges();

            return Ok(course);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseExists(int id)
        {
            return db.Courses.Count(e => e.CourseId == id) > 0;
        }
    }
}