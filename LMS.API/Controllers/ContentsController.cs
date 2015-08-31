// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentsController.cs" company="">
//   
// </copyright>
// <summary>
//   The contents controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    using LMS.API.Contexts;
    using LMS.API.Models;

    /// <summary>
    /// The contents controller.
    /// </summary>
    public class ContentsController : ApiBaseController
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LMSContext db = new LMSContext();

        // PUT: api/Contents/5        
        // [HttpPut]
        // public IHttpActionResult UpdateContent(int id, Content content)
        // {
        // if (!ModelState.IsValid)
        // {
        // return BadRequest(ModelState);
        // }

        // if (id != content.ContentId)
        // {
        // return BadRequest();
        // }

        // db.Entry(content).State = EntityState.Modified;

        // try
        // {
        // db.SaveChanges();
        // }
        // catch (DbUpdateConcurrencyException)
        // {
        // if (!ContentExists(id))
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
        /// The create content.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        public async Task<IHttpActionResult> CreateContent(Content content)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.db.Contents.Add(content);
            this.db.SaveChanges();

            return this.Ok(content);
        }

        /// <summary>
        /// The delete content.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteContent(int id)
        {
            Content content = this.db.Contents.Find(id);
            if (content == null)
            {
                return this.NotFound();
            }

            this.db.Contents.Remove(content);
            this.db.SaveChanges();

            return this.Ok(content);
        }

        /// <summary>
        /// The get content by course.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IHttpActionResult> GetContentByCourse(int id)
        {
            IEnumerable<Content> contents = this.db.Contents
                .Where(c => c.CourseId == id)
                .Select(c => new
                {
                    c.ContentId, 
                    c.Name, 
                    c.Description, 
                    c.Resource
                })
                .ToList()
                .Select(c => new Content
                {
                    ContentId = c.ContentId, 
                    Name = c.Name, 
                    Description = c.Description, 
                    Resource = c.Resource
                });

            return this.Ok(contents);
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
        /// The content exists.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool ContentExists(int id)
        {
            return this.db.Contents.Count(e => e.ContentId == id) > 0;
        }
    }
}