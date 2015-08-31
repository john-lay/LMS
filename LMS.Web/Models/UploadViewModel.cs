// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UploadViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The upload view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    /// <summary>
    /// The upload view model.
    /// </summary>
    public class UploadViewModel
    {
        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        public HttpPostedFileBase File { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}