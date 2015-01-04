using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.API.Models
{
    public class Email
    {
        public string[] Recipients { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}