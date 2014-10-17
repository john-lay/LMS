using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.API.Models
{
    [Table("UserGroup")]
    public class UserGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserGroupId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        // -1 for root
        public int ParentId { get; set; }
    }
}


