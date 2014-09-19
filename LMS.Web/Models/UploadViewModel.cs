﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Web.Models
{
    public class UploadViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}