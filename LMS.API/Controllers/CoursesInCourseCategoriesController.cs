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

namespace LMS.API.Controllers
{
    public class CoursesInCourseCategoriesController : ApiController
    {
        private LMSContext db = new LMSContext();

        // GET: api/CoursesInCourseCategories
        public IQueryable<CoursesInCourseCategory> GetCoursesInCourseCategories()
        {
            return db.CoursesInCourseCategories;
        }

        // GET: api/CoursesInCourseCategories/5
        [ResponseType(typeof(CoursesInCourseCategory))]
        public IHttpActionResult GetCoursesInCourseCategory(int id)
        {
            CoursesInCourseCategory coursesInCourseCategory = db.CoursesInCourseCategories.Find(id);
            if (coursesInCourseCategory == null)
            {
                return NotFound();
            }

            return Ok(coursesInCourseCategory);
        }

        // PUT: api/CoursesInCourseCategories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCoursesInCourseCategory(int id, CoursesInCourseCategory coursesInCourseCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != coursesInCourseCategory.CourseCategoryId)
            {
                return BadRequest();
            }

            db.Entry(coursesInCourseCategory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoursesInCourseCategoryExists(id))
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

        // POST: api/CoursesInCourseCategories
        [ResponseType(typeof(CoursesInCourseCategory))]
        public IHttpActionResult PostCoursesInCourseCategory(CoursesInCourseCategory coursesInCourseCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CoursesInCourseCategories.Add(coursesInCourseCategory);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CoursesInCourseCategoryExists(coursesInCourseCategory.CourseCategoryId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = coursesInCourseCategory.CourseCategoryId }, coursesInCourseCategory);
        }

        // DELETE: api/CoursesInCourseCategories/5
        [ResponseType(typeof(CoursesInCourseCategory))]
        public IHttpActionResult DeleteCoursesInCourseCategory(int id)
        {
            CoursesInCourseCategory coursesInCourseCategory = db.CoursesInCourseCategories.Find(id);
            if (coursesInCourseCategory == null)
            {
                return NotFound();
            }

            db.CoursesInCourseCategories.Remove(coursesInCourseCategory);
            db.SaveChanges();

            return Ok(coursesInCourseCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CoursesInCourseCategoryExists(int id)
        {
            return db.CoursesInCourseCategories.Count(e => e.CourseCategoryId == id) > 0;
        }
    }
}