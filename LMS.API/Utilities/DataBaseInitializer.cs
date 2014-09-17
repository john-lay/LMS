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
            db.SaveChanges();

            User user = new User { ClientId = 1, FirstName = "Joe", LastName = "Bloggs", EmailAddress = "joe.bloggs@test.com", Password = "somehash" };
            db.Users.Add(user);
            db.SaveChanges();
        }
    }
}