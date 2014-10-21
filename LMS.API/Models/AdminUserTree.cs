using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.API.Models
{
    public class AdminUserTree
    {
        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public List<User> Users { get; set; }
    }
}