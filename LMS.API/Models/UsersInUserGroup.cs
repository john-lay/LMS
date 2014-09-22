using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.API.Models
{
    [Table("UsersInUserGroup")]
    public class UsersInUserGroup
    {
        [Key, Column(Order = 0)]
        [ForeignKey("UserGroup")]
        public int UserGroupId { get; set; }

        public virtual UserGroup UserGroup { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}