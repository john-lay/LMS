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

namespace LMS.API.Controllers
{
    public class UsersController : ApiController
    {
        private LMSContext db = new LMSContext();

        // GET: api/Users
        //public IQueryable<User> GetUsers()
        //{
        //    return db.Users;
        //}

        // GET: api/Users/5
        [HttpGet]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new { FirstName = user.FirstName, LastName = user.LastName});
        }

        //[HttpGet]
        public IHttpActionResult GetUsersByClient(int id)
        {
            IEnumerable<User> users = db.Users
                .Where(u => u.ClientId == id)
                .Select(u => new // use projection here to create an anonymous object. Then below return a new User object as a subset
                { 
                    ClientId = u.ClientId,
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    EmailAddress = u.EmailAddress
                    //ClientObj = u.Client,
                    //Password = u.Password
                }) 
                .ToList()
                .Select(u => new User 
                {
                    ClientId = u.ClientId,
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    EmailAddress = u.EmailAddress
                });

            return Ok(users);
        }

        // PUT: api/Users/5
        [HttpPut]
        public IHttpActionResult UpdateUser(int id, [FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [HttpPost]
        public IHttpActionResult CreateUser([FromBody]User[] users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (User user in users)
            {
                db.Users.Add(user);
                db.SaveChanges();
            }

            return Ok(users);
            //return CreatedAtRoute("DefaultApi", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}