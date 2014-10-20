using LMS.API.Contexts;
using LMS.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace LMS.API.Controllers
{
    public class ContentsController : ApiBaseController
    {
        private LMSContext db = new LMSContext();

        public IHttpActionResult GetContentByCourse(int id)
        {
            IEnumerable<Content> contents = db.Contents
                .Where(c => c.CourseId == id)
                .Select(c => new
                {
                    ContentId = c.ContentId,
                    Name = c.Name,
                    Description = c.Description,
                    Resource = c.Resource
                })
                .ToList()
                .Select(c => new Content
                {
                    ContentId = c.ContentId,
                    Name = c.Name,
                    Description = c.Description,
                    Resource = c.Resource
                });

            return Ok(contents);
        }

        // PUT: api/Contents/5        
        //[HttpPut]
        //public IHttpActionResult UpdateContent(int id, Content content)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != content.ContentId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(content).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ContentExists(id))
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

        // POST: api/Contents
        [HttpPost]
        public IHttpActionResult CreateContent(Content content)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Contents.Add(content);
            db.SaveChanges();

            return Ok(content);
        }

        // DELETE: api/Contents/5
        [HttpDelete]
        public IHttpActionResult DeleteContent(int id)
        {
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return NotFound();
            }

            db.Contents.Remove(content);
            db.SaveChanges();

            return Ok(content);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContentExists(int id)
        {
            return db.Contents.Count(e => e.ContentId == id) > 0;
        }
    }
}