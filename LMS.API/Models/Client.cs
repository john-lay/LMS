// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Client.cs" company="">
//   
// </copyright>
// <summary>
//   The client.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The client.
    /// </summary>
    [Table("Client")]
    public class Client
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }

        /// <summary>
        /// Gets or sets the logo resource.
        /// </summary>
        [StringLength(50)]
        public string LogoResource { get; set; }

        /// <summary>
        /// Gets or sets the logo title.
        /// </summary>
        [StringLength(50)]
        public string LogoTitle { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        #endregion
    }
}