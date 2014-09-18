using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using LMS.API.Models;

namespace LMS.Services.Contexts
{
    public class LMSContext : DbContext
    {
        public LMSContext()
            : base("name=LMSContext") // this uses the "LMSContext" connection string from the config
        {
            //
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<UsersInGroup> GroupUser { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Content> Contents { get; set; }
    }
}