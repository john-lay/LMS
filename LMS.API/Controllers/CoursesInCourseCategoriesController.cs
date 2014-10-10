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
using LMS.API.Contexts;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace LMS.API.Controllers
{
    [EnableCors(origins: "http://localhost:58733", headers: "*", methods: "*")]
    public class CoursesInCourseCategoriesController : ApiController
    {
        private LMSContext db = new LMSContext();

        /// <summary>
        /// GET: api/CoursesInCourseCategories
        /// </summary>
        /// <param name="id">Client ID</param>
        /// <returns></returns>
        public string GetCourseCategoriesAndCourses(int id)
        {
            List<CourseCategoryNode> courseCategoryTree = new List<CourseCategoryNode>();
            var categories = db.CourseCategories;

            foreach (var cat in categories)
            {
                var query = from coursesInCourseCat in db.CoursesInCourseCategories
                        join course in db.Courses on coursesInCourseCat.CourseId equals course.CourseId
                        where coursesInCourseCat.CourseCategoryId == cat.CourseCategoryId && course.ClientId == id
                        select course;

                courseCategoryTree.Add(new CourseCategoryNode
                {
                    Category = cat,
                    Courses = query.ToArray()
                });
            }

            // normalize courseCategoryTree for Kendo Tree
            var returnKendo = courseCategoryTree
                .Select(x => new 
                {
                    id = x.Category.CourseCategoryId,
                    text = x.Category.Name,
                    expanded = true,
                    items = x.Courses.Select(c => new { id = c.CourseId, text = c.Name }).ToArray()
                });

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(returnKendo);
        }

        // PUT: api/CoursesInCourseCategories/5
        //[HttpPut]
        //public IHttpActionResult UpdateCoursesInCourseCategory(int id, CoursesInCourseCategory coursesInCourseCategory)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != coursesInCourseCategory.CourseCategoryId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(coursesInCourseCategory).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CoursesInCourseCategoryExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/CoursesInCourseCategories
        [HttpPost]
        public IHttpActionResult AddCourseToCourseCategory(CoursesInCourseCategory coursesInCourseCategory)
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

            return Ok(coursesInCourseCategory);
        }

        // DELETE: api/CoursesInCourseCategories/5
        [HttpDelete]
        public IHttpActionResult DeleteCourseInCourseCategory(CoursesInCourseCategory model)
        {
            CoursesInCourseCategory coursesInCourseCategory = db.CoursesInCourseCategories
                .First(c => c.CourseId == model.CourseId && c.CourseCategoryId == model.CourseCategoryId);

            if (coursesInCourseCategory == null)
            {
                return NotFound();
            }

            db.CoursesInCourseCategories.Remove(coursesInCourseCategory);
            db.SaveChanges();

            // now delete course too
            var resource = new CoursesController();
            if (resource.DeleteCourse(model.CourseId) == NotFound())
            {
                return NotFound();
            }

            // TODO: delete all course content for this course
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