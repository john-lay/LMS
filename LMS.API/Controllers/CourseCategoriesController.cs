// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CourseCategoriesController.cs" company="">
//   
// </copyright>
// <summary>
//   The course categories controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Controllers
{
    using System;
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
    /// The course categories controller.
    /// </summary>
    public class CourseCategoriesController : ApiBaseController
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LMSContext db = new LMSContext();

        /// <summary>
        /// The create course category.
        /// </summary>
        /// <param name="courseCategory">
        /// The course category.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        public async Task<IHttpActionResult> CreateCourseCategory(CourseCategory courseCategory)
        {
            courseCategory.ClientId = this.ClientId;

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.db.CourseCategories.Add(courseCategory);
            this.db.SaveChanges();

            return this.Ok(courseCategory);
        }

        /// <summary>
        /// The delete course category.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteCourseCategory(int id)
        {
            CourseCategory courseCategory = this.db.CourseCategories.Find(id);

            if (courseCategory == null)
            {
                return this.NotFound();
            }

            try
            {
                // try removing associated courses before deleting course category
                IQueryable<CoursesInCourseCategory> ccc = this.db.CoursesInCourseCategories.Where(c => c.CourseCategoryId == id);
                this.db.CoursesInCourseCategories.RemoveRange(ccc);
                this.db.SaveChanges();

                this.db.CourseCategories.Remove(courseCategory);
                this.db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    throw new Exception("Please DELETE all courses in this category before attempting to delete the category.", ex);
                }
            }

            return this.Ok(courseCategory);
        }

        /// <summary>
        /// The get course categories.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<string> GetCourseCategories()
        {
            // return db.CourseCategories;
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(this.db.CourseCategories
                .Where(cc => cc.ClientId == this.ClientId)
                .Select(x => new
                {
                    id = x.CourseCategoryId, text = x.Name
                }).ToArray());
        }

        /// <summary>
        /// The get course category.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetCourseCategory(int id)
        {
            CourseCategory courseCategory = this.db.CourseCategories.Find(id);
            if (courseCategory == null)
            {
                return this.NotFound();
            }

            return this.Ok(courseCategory);
        }

        /// <summary>
        /// The update course category.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="courseCategory">
        /// The course category.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        public async Task<IHttpActionResult> UpdateCourseCategory(int id, CourseCategory courseCategory)
        {
            courseCategory.ClientId = this.ClientId;

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != courseCategory.CourseCategoryId)
            {
                return this.BadRequest();
            }

            this.db.Entry(courseCategory).State = EntityState.Modified;

            try
            {
                this.db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.CourseCategoryExists(id))
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
        /// The course category exists.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool CourseCategoryExists(int id)
        {
            return this.db.CourseCategories.Count(e => e.CourseCategoryId == id) > 0;
        }
    }
}