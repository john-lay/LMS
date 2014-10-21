using LMS.API.Contexts;
using LMS.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LMS.API.Utilities
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<LMSContext>
    {
        private AuthContext context;

        private UserManager<IdentityUser> userManager;

        protected override void Seed(LMSContext db)
        {
            context = new AuthContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(context));

            // create admin in AspNetUsers table
            IdentityUser adminUser = new IdentityUser { UserName = "admin@avemtec.com" };
            userManager.Create(adminUser, "SuperPass");
            
            // create admin role
            context.Roles.Add(new IdentityRole { Name = "SuperAdmin" });
            context.Roles.Add(new IdentityRole { Name = "Admin" });
            context.Roles.Add(new IdentityRole { Name = "User" });
            context.SaveChanges();

            // add user to role
            userManager.AddToRole(adminUser.Id, "SuperAdmin");

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // seed db with dummy data
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // client
            Client client = new Client { ClientId = 1, Name = "Avemtec" };
            db.Clients.Add(client);
            Client client1 = new Client { ClientId = 2, Name = "Apple", LogoTitle = "Apple Logo", LogoResource = "/path/to/resource" };
            db.Clients.Add(client1);

            // create admin in User table
            // DO NOT DELETE ----------------------------------------------------------------------------------------------------
            User admin = new User { ClientId = 1, FirstName = "Super", LastName = "Admin", ASPNetUserId = adminUser.Id, EmailAddress = "admin@avemtec.com" };
            db.Users.Add(admin);

            // users
            IdentityUser aspuser = new IdentityUser { UserName = "steve.jobs@apple.com" };
            userManager.Create(aspuser, "password");
            User user = new User { ClientId = 2, FirstName = "Steve", LastName = "jobs", ASPNetUserId = aspuser.Id, EmailAddress = "steve.jobs@apple.com" };
            db.Users.Add(user);            

            IdentityUser aspuser2 = new IdentityUser { UserName = "Scott.Hanselman@test.com" };
            userManager.Create(aspuser2, "password");
            User user2 = new User { ClientId = 2, FirstName = "Scott", LastName = "Hanselman", ASPNetUserId = aspuser2.Id, EmailAddress = "Scott.Hanselman@test.com" };
            db.Users.Add(user2);

            IdentityUser aspuser3 = new IdentityUser { UserName = "joe.bloggs@test.com" };
            userManager.Create(aspuser3, "password");
            User user3 = new User { ClientId = 2, FirstName = "Joe", LastName = "Bloggs", ASPNetUserId = aspuser3.Id, EmailAddress = "joe.bloggs@test.com" };
            db.Users.Add(user3);

            // add user to role
            userManager.AddToRole(aspuser.Id, "Admin");
            userManager.AddToRole(aspuser2.Id, "User");
            userManager.AddToRole(aspuser3.Id, "User");

            // user group
            // DO NOT DELETE ----------------------------------------------------------------------------------------------------
            UserGroup adminGroup = new UserGroup { UserGroupId = 1, Name = "Administrators", ParentId = -1, ClientId = 1 };
            db.UserGroups.Add(adminGroup);

            UserGroup group = new UserGroup {  UserGroupId = 2, Name = "cohorts", ParentId = -1, ClientId = 2 };
            db.UserGroups.Add(group);
            UsersInUserGroup ug = new UsersInUserGroup { UserGroupId = 2, UserId = 1 };
            db.UsersInUserGroups.Add(ug);
            UsersInUserGroup ug2 = new UsersInUserGroup { UserGroupId = 2, UserId = 2 };
            db.UsersInUserGroups.Add(ug2);

            // courses / course content
            Course course = new Course { CourseId = 1, ClientId = 2, Name = "HTML Fundamentals", Description = "introduction to HTML description" };
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

        public AuthContext _context { get; set; }
    }
}