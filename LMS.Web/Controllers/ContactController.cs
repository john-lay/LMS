// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContactController.cs" company="">
//   
// </copyright>
// <summary>
//   The contact controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    /// <summary>
    /// The contact controller.
    /// </summary>
    public class ContactController : LmsBaseController
    {
        /// <summary>
        /// The email.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ActionResult> Email()
        {
            return this.View();
        }
    }
}