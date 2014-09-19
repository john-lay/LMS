using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using LMS.API.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace LMS.Services.Contexts
{
    public class LMSContext : DbContext
    {
        public LMSContext()
            : base("name=LMSContext") // this uses the "LMSContext" connection string from the config
        {
            //
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        public DbSet<UsersInUserGroup> UsersInUserGroups { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Content> Contents { get; set; }

        public DbSet<CourseGroup> CourseGroups { get; set; }

        public DbSet<CoursesInCourseGroup> CoursesInCourseGroups { get; set; }

        public DbSet<CourseSession> CourseSessions { get; set; }

        public DbSet<UsersInCourseSession> UsersInCourseSessions { get; set; }
    }
}