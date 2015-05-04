using LMS.API.Contexts;
using LMS.API.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace LMS.API.Controllers
{
    using System;

    public class CourseCategoriesController : ApiBaseController
    {
        private LMSContext db = new LMSContext();

        // GET: api/CourseCategories
        public string GetCourseCategories()
        {
            //return db.CourseCategories;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(
                db.CourseCategories
                .Where(cc => cc.ClientId == this.ClientId)
                .Select(x => new { id = x.CourseCategoryId, text = x.Name })
                .ToArray());
        }

        // GET: api/CourseCategories/5
        [HttpGet]
        public IHttpActionResult GetCourseCategory(int id)
        {
            CourseCategory courseCategory = db.CourseCategories.Find(id);
            if (courseCategory == null)
            {
                return NotFound();
            }

            return Ok(courseCategory);
        }

        // PUT: api/CourseCategories/5
        [HttpPut]
        public IHttpActionResult UpdateCourseCategory(int id, CourseCategory courseCategory)
        {
            courseCategory.ClientId = this.ClientId;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != courseCategory.CourseCategoryId)
            {
                return BadRequest();
            }

            db.Entry(courseCategory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseCategoryExists(id))
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

        // POST: api/CourseCategories
        [HttpPost]
        public IHttpActionResult CreateCourseCategory(CourseCategory courseCategory)
        {
            courseCategory.ClientId = this.ClientId;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CourseCategories.Add(courseCategory);
            db.SaveChanges();

            return Ok(courseCategory);
        }

        // DELETE: api/CourseCategories/5
        [HttpDelete]
        public IHttpActionResult DeleteCourseCategory(int id)
        {
            CourseCategory courseCategory = db.CourseCategories.Find(id);

            if (courseCategory == null)
            {
                return NotFound();
            }

            try
            {
                // try removing associated courses before deleting course category
                var ccc = db.CoursesInCourseCategories.Where(c => c.CourseCategoryId == id);
                db.CoursesInCourseCategories.RemoveRange(ccc);
                db.SaveChanges();

                db.CourseCategories.Remove(courseCategory);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    throw new Exception("Please DELETE all courses in this category before attempting to delete the category.", ex);
                }
            }

            return Ok(courseCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseCategoryExists(int id)
        {
            return db.CourseCategories.Count(e => e.CourseCategoryId == id) > 0;
        }
    }
}