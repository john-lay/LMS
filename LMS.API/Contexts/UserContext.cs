using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using LMS.API.Entities;

namespace LMS.Services.Contexts
{
    public class UserContext : DbContext
    {
        public UserContext()
            : base("name=UserContext") // this uses the "UserContext" connection string from the config
        {
            //
        }

        public DbSet<User> Users { get; set; }
    }
}