// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataBaseInitializer.cs" company="">
//   
// </copyright>
// <summary>
//   The database initializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Utilities
{
    using System;
    using System.Data.Entity;

    using LMS.API.Contexts;
    using LMS.API.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    /// <summary>
    /// The database initializer.
    /// </summary>
    public class DatabaseInitializer : CreateDatabaseIfNotExists<LMSContext>
    {
        #region Fields

        /// <summary>
        /// The context.
        /// </summary>
        private AuthContext context;

        /// <summary>
        /// The user manager.
        /// </summary>
        private UserManager<IdentityUser> userManager;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the _context.
        /// </summary>
        public AuthContext _context { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The seed.
        /// </summary>
        /// <param name="db">
        /// The db.
        /// </param>
        protected override void Seed(LMSContext db)
        {
            this.context = new AuthContext();
            this.userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(this.context));

            // create admin in AspNetUsers table
            var adminUser = new IdentityUser
                                {
                                    UserName = "admin@avemtec.com"
                                };
            this.userManager.Create(adminUser, "SuperPass");

            // create admin role
            this.context.Roles.Add(new IdentityRole
                                       {
                                           Name = "SuperAdmin"
                                       });
            this.context.Roles.Add(new IdentityRole
                                       {
                                           Name = "Admin"
                                       });
            this.context.Roles.Add(new IdentityRole
                                       {
                                           Name = "User"
                                       });
            this.context.SaveChanges();

            // add user to role
            this.userManager.AddToRole(adminUser.Id, "SuperAdmin");

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // seed db with dummy data
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // client
            var client = new Client
                             {
                                 ClientId = 1, Name = "Avemtec"
                             };
            db.Clients.Add(client);
            var client1 = new Client
                              {
                                  ClientId = 2, Name = "Apple", LogoTitle = "Apple Logo", LogoResource = "/path/to/resource"
                              };
            db.Clients.Add(client1);

            // create admin in User table
            // DO NOT DELETE ----------------------------------------------------------------------------------------------------
            var admin = new User
                            {
                                ClientId = 1, FirstName = "Super", LastName = "Admin", ASPNetUserId = adminUser.Id, EmailAddress = "admin@avemtec.com"
                            };
            db.Users.Add(admin);

            // users
            var aspuser = new IdentityUser
                              {
                                  UserName = "steve.jobs@apple.com"
                              };
            this.userManager.Create(aspuser, "password");
            var user = new User
                           {
                               ClientId = 2, FirstName = "Steve", LastName = "jobs", ASPNetUserId = aspuser.Id, EmailAddress = "steve.jobs@apple.com"
                           };
            db.Users.Add(user);

            var aspuser2 = new IdentityUser
                               {
                                   UserName = "Scott.Hanselman@test.com"
                               };
            this.userManager.Create(aspuser2, "password");
            var user2 = new User
                            {
                                ClientId = 2, FirstName = "Scott", LastName = "Hanselman", ASPNetUserId = aspuser2.Id, EmailAddress = "Scott.Hanselman@test.com"
                            };
            db.Users.Add(user2);

            var aspuser3 = new IdentityUser
                               {
                                   UserName = "joe.bloggs@test.com"
                               };
            this.userManager.Create(aspuser3, "password");
            var user3 = new User
                            {
                                ClientId = 2, FirstName = "Joe", LastName = "Bloggs", ASPNetUserId = aspuser3.Id, EmailAddress = "joe.bloggs@test.com"
                            };
            db.Users.Add(user3);

            // add user to role
            this.userManager.AddToRole(aspuser.Id, "Admin");
            this.userManager.AddToRole(aspuser2.Id, "User");
            this.userManager.AddToRole(aspuser3.Id, "User");

            // user group
            // DO NOT DELETE ----------------------------------------------------------------------------------------------------
            var adminGroup = new UserGroup
                                 {
                                     UserGroupId = 1, Name = "Administrators", ParentId = -1, ClientId = 1
                                 };
            db.UserGroups.Add(adminGroup);

            var group = new UserGroup
                            {
                                UserGroupId = 2, Name = "cohorts", ParentId = -1, ClientId = 2
                            };
            db.UserGroups.Add(group);
            var ug = new UsersInUserGroup
                         {
                             UserGroupId = 2, UserId = 1
                         };
            db.UsersInUserGroups.Add(ug);
            var ug2 = new UsersInUserGroup
                          {
                              UserGroupId = 2, UserId = 2
                          };
            db.UsersInUserGroups.Add(ug2);

            // courses / course content
            var course = new Course
                             {
                                 CourseId = 1, ClientId = 2, Name = "HTML Fundamentals", Description = "introduction to HTML description"
                             };
            db.Courses.Add(course);
            var content = new Content
                              {
                                  CourseId = 1, Name = "My PDF", Description = "Please read this pdf", Resource = "/path/to/resource"
                              };
            db.Contents.Add(content);

            // course group
            var cg = new CourseCategory
                         {
                             CourseCategoryId = 1, Name = "Web technology", ClientId = 1
                         };
            db.CourseCategories.Add(cg);
            var ccg = new CoursesInCourseCategory
                          {
                              CourseCategoryId = 1, CourseId = 1
                          };
            db.CoursesInCourseCategories.Add(ccg);

            // course session
            var cs = new CourseSession
                         {
                             CourseId = 1, IsRolling = false, StartDate = new DateTime(2014, 09, 01), EndDate = new DateTime(2014, 11, 01)
                         };
            db.CourseSessions.Add(cs);
            var ucs = new UsersInCourseSession
                          {
                              CourseSessionId = 1, UserId = 1
                          };
            db.UsersInCourseSessions.Add(ucs);
            var ucs2 = new UsersInCourseSession
                           {
                               CourseSessionId = 1, UserId = 2
                           };
            db.UsersInCourseSessions.Add(ucs2);

            db.SaveChanges();
        }

        #endregion
    }
}