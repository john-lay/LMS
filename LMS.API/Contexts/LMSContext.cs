// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LMSContext.cs" company="">
//   
// </copyright>
// <summary>
//   The lms context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Contexts
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    using LMS.API.Models;

    /// <summary>
    /// The lms context.
    /// </summary>
    public class LMSContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LMSContext"/> class.
        /// </summary>
        public LMSContext()
            : base("name=LMSContext")
        {
            // this uses the "LMSContext" connection string from the config
        }

        /// <summary>
        /// Gets or sets the clients.
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Gets or sets the contents.
        /// </summary>
        public DbSet<Content> Contents { get; set; }

        /// <summary>
        /// Gets or sets the course categories.
        /// </summary>
        public DbSet<CourseCategory> CourseCategories { get; set; }

        /// <summary>
        /// Gets or sets the course sessions.
        /// </summary>
        public DbSet<CourseSession> CourseSessions { get; set; }

        /// <summary>
        /// Gets or sets the courses.
        /// </summary>
        public DbSet<Course> Courses { get; set; }

        /// <summary>
        /// Gets or sets the courses in course categories.
        /// </summary>
        public DbSet<CoursesInCourseCategory> CoursesInCourseCategories { get; set; }

        /// <summary>
        /// Gets or sets the user groups.
        /// </summary>
        public DbSet<UserGroup> UserGroups { get; set; }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the users in course sessions.
        /// </summary>
        public DbSet<UsersInCourseSession> UsersInCourseSessions { get; set; }

        /// <summary>
        /// Gets or sets the users in user groups.
        /// </summary>
        public DbSet<UsersInUserGroup> UsersInUserGroups { get; set; }

        /// <summary>
        /// The on model creating.
        /// </summary>
        /// <param name="modelBuilder">
        /// The model builder.
        /// </param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}