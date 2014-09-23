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
using System.Web.Http.Cors;

namespace LMS.API.Controllers
{
    [EnableCors(origins: "http://localhost:58733", headers: "*", methods: "*")]
    public class CourseCategoriesController : ApiController
    {
        private LMSContext db = new LMSContext();

        // GET: api/CourseCategories
        public IQueryable<CourseCategory> GetCourseCategories()
        {
            return db.CourseCategories;
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

            db.CourseCategories.Remove(courseCategory);
            db.SaveChanges();

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