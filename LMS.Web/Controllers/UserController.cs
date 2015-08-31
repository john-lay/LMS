// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserController.cs" company="">
//   
// </copyright>
// <summary>
//   The user controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    /// <summary>
    /// The user controller.
    /// </summary>
    public class UserController : LmsBaseController
    {
        /// <summary>
        /// The admin.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ActionResult> Admin()
        {
            return this.View();
        }

        /// <summary>
        /// The group.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ActionResult> Group()
        {
            return this.View();
        }

        /// <summary>
        /// The manage.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ActionResult> Manage()
        {
            return this.View();
        }
    }
}