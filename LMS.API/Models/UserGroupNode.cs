using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.API.Models
{
    public class UserGroupNode
    {
        public UserGroup UserGroup { get; set; }

        public User[] Users { get; set; }
    }
}