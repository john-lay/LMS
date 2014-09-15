using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.API.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string Password { get; set; }
    }
}