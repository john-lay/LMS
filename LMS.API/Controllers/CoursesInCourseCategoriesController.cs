// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CoursesInCourseCategoriesController.cs" company="">
//   
// </copyright>
// <summary>
//   The courses in course categories controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Controllers
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Script.Serialization;

    using LMS.API.Contexts;
    using LMS.API.Models;

    /// <summary>
    /// The courses in course categories controller.
    /// </summary>
    public class CoursesInCourseCategoriesController : ApiBaseController
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LMSContext db = new LMSContext();

        // PUT: api/CoursesInCourseCategories/5
        // [HttpPut]
        // public IHttpActionResult UpdateCoursesInCourseCategory(int id, CoursesInCourseCategory coursesInCourseCategory)
        // {
        // if (!ModelState.IsValid)
        // {
        // return BadRequest(ModelState);
        // }

        // if (id != coursesInCourseCategory.CourseCategoryId)
        // {
        // return BadRequest();
        // }

        // db.Entry(coursesInCourseCategory).State = EntityState.Modified;

        // try
        // {
        // db.SaveChanges();
        // }
        // catch (DbUpdateConcurrencyException)
        // {
        // if (!CoursesInCourseCategoryExists(id))
        // {
        // return NotFound();
        // }
        // else
        // {
        // throw;
        // }
        // }

        // return StatusCode(HttpStatusCode.NoContent);
        // }

        /// <summary>
        /// The add course to course category.
        /// </summary>
        /// <param name="coursesInCourseCategory">
        /// The courses in course category.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        public async Task<IHttpActionResult> AddCourseToCourseCategory(CoursesInCourseCategory coursesInCourseCategory)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.db.CoursesInCourseCategories.Add(coursesInCourseCategory);

            try
            {
                this.db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (this.CoursesInCourseCategoryExists(coursesInCourseCategory.CourseCategoryId))
                {
                    return this.Conflict();
                }

                throw;
            }

            return this.Ok(coursesInCourseCategory);
        }

        /// <summary>
        /// The delete course in course category.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCourseInCourseCategory(CoursesInCourseCategory model)
        {
            CoursesInCourseCategory coursesInCourseCategory = this.db.CoursesInCourseCategories.First(c => c.CourseId == model.CourseId && c.CourseCategoryId == model.CourseCategoryId);

            if (coursesInCourseCategory == null)
            {
                return this.NotFound();
            }

            this.db.CoursesInCourseCategories.Remove(coursesInCourseCategory);
            this.db.SaveChanges();

            // now delete course too
            var resource = new CoursesController();
            if (await resource.DeleteCourse(model.CourseId) == this.NotFound())
            {
                return this.NotFound();
            }

            // TODO: delete all course content for this course
            return this.Ok(coursesInCourseCategory);
        }

        /// <summary>
        /// GET: api/CoursesInCourseCategories
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<string> GetCourseCategoriesAndCourses()
        {
            var courseCategoryTree = new List<CourseCategoryNode>();
            IQueryable<CourseCategory> categories = this.db.CourseCategories.Where(cc => cc.ClientId == this.ClientId);

            foreach (CourseCategory cat in categories)
            {
                IQueryable<Course> query = from coursesInCourseCat in this.db.CoursesInCourseCategories 
                                           join course in this.db.Courses on coursesInCourseCat.CourseId equals course.CourseId 
                                           where coursesInCourseCat.CourseCategoryId == cat.CourseCategoryId && course.ClientId == this.ClientId select course;

                courseCategoryTree.Add(new CourseCategoryNode
                                           {
                                               Category = cat, Courses = query.ToArray()
                                           });
            }

            // normalize courseCategoryTree for Kendo Tree
            var returnKendo = courseCategoryTree
                .Select(x => new
                {
                    id = x.Category.CourseCategoryId, 
                    text = x.Category.Name, 
                    expanded = true, 
                    items = x.Courses.Select(c => new
                    {
                        id = c.CourseId, 
                        text = c.Name
                    })
                    .ToArray()
                });

            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(returnKendo);
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
        /// The courses in course category exists.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CoursesInCourseCategoryExists(int id)
        {
            return this.db.CoursesInCourseCategories.Count(e => e.CourseCategoryId == id) > 0;
        }
    }
}