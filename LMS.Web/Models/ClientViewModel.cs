// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The client view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The client view model.
    /// </summary>
    public class ClientViewModel
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}