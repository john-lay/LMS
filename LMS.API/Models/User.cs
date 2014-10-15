using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMS.API.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        //[ForeignKey("ASPNetUsers")]
        public string ASPNetUserId { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string EmailAddress { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        public DateTime? Locked { get; set; }

        public bool IsLocked 
        { 
            get
            {
                return Locked != null;
            } 
        }
    }
}