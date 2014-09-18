﻿using System;
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
    public class CoursesController : ApiController
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

        // PUT: api/Courses/5
        [HttpPut]
        public IHttpActionResult UpdateCourse(int id, Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.CourseId)
            {
                return BadRequest();
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