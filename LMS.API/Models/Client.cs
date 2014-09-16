using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.API.Models
{
    [Table("Client")]
    public class Client
    {
        public int ClientId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string LogoTitle { get; set; }

        [StringLength(50)]
        public string LogoResource { get; set; }
    }
}