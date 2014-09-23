using LMS.API.Models;
using LMS.Services.Contexts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LMS.API.Utilities
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<LMSContext>
    {
        protected override void Seed(LMSContext db)
        {
            // client
            Client client = new Client { ClientId = 1, Name="Apple", LogoTitle = "Apple Logo", LogoResource = "/path/to/resource"};
            db.Clients.Add(client);

            // users
            User user = new User { ClientId = 1, FirstName = "Joe", LastName = "Bloggs", EmailAddress = "joe.bloggs@test.com", Password = "somehash" };
            db.Users.Add(user);
            User user2 = new User { ClientId = 1, FirstName = "Scott", LastName = "Hanselman", EmailAddress = "Scott.Hanselman@test.com", Password = "someotherhash" };
            db.Users.Add(user2);

            // user group
            UserGroup group = new UserGroup {  UserGroupId = 1, Name = "cohorts", ParentId = -1 };
            db.UserGroups.Add(group);
            UsersInUserGroup ug = new UsersInUserGroup { UserGroupId = 1, UserId = 1 };
            db.UsersInUserGroups.Add(ug);
            UsersInUserGroup ug2 = new UsersInUserGroup { UserGroupId = 1, UserId = 2 };
            db.UsersInUserGroups.Add(ug2);

            // courses / course content
            Course course = new Course { CourseId = 1, ClientId = 1, Name = "HTML Fundamentals", Description = "introduction to HTML description" };
            db.Courses.Add(course);
            Content content = new Content { CourseId = 1, Name = "My PDF", Description = "Please read this pdf", Resource = "/path/to/resource" };
            db.Contents.Add(content);

            //course group
            CourseCategory cg = new CourseCategory { CourseCategoryId = 1, Name = "Web technology" };
            db.CourseCategories.Add(cg);
            CoursesInCourseCategory ccg = new CoursesInCourseCategory { CourseCategoryId = 1, CourseId = 1 };
            db.CoursesInCourseCategories.Add(ccg);

            // course session
            CourseSession cs = new CourseSession { CourseId = 1, IsRolling = false, StartDate = new DateTime(2014, 09, 01), EndTime = new DateTime(2014, 11, 01) };
            db.CourseSessions.Add(cs);
            UsersInCourseSession ucs = new UsersInCourseSession { CourseSessionId = 1, UserId = 1 };
            db.UsersInCourseSessions.Add(ucs);
            UsersInCourseSession ucs2 = new UsersInCourseSession { CourseSessionId = 1, UserId = 2 };
            db.UsersInCourseSessions.Add(ucs2);

            db.SaveChanges();
        }
    }
}