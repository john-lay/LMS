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
            Client client = new Client { ClientId = 1, Name="Apple", LogoTitle = "Apple Logo", LogoResource = "/path/to/resource"};
            db.Clients.Add(client);
            User user = new User { ClientId = 1, FirstName = "Joe", LastName = "Bloggs", EmailAddress = "joe.bloggs@test.com", Password = "somehash" };
            db.Users.Add(user);
            User user2 = new User { ClientId = 1, FirstName = "Scott", LastName = "Hanselman", EmailAddress = "Scott.Hanselman@test.com", Password = "someotherhash" };
            db.Users.Add(user2);
            Group group = new Group {  GroupId = 1, GroupName = "cohorts", ParentId = -1 };
            db.Groups.Add(group);
            UsersInGroup ug = new UsersInGroup { GroupId = 1, UserId = 1 };
            db.GroupUser.Add(ug);
            UsersInGroup ug2 = new UsersInGroup { GroupId = 1, UserId = 2 };
            db.GroupUser.Add(ug2);
            Course course = new Course { ClientId = 1, Name = "HTML Fundamentals", Description = "introduction to HTML description" };
            db.Courses.Add(course);
            Content content = new Content { CourseId = 1, Name = "My PDF", Description = "Please read this pdf", Resource = "/path/to/resource" };
            db.Users.Add(user);

            db.SaveChanges();
        }
    }
}