using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Web.Infrastructure
{
    public class UserProfile
    {
        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}