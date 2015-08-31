// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="">
//   
// </copyright>
// <summary>
//   The home controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Security;

    /// <summary>
    /// The home controller.
    /// </summary>
    public class HomeController : LmsBaseController
    {
        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return this.View();
        }

        /// <summary>
        /// The logout.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ActionResult> Logout()
        {
            this.Session.Clear();
            this.Session.Abandon();
            FormsAuthentication.SignOut();

            return this.View("Index");
        }

        /// <summary>
        /// The welcome.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ActionResult> Welcome()
        {
            return this.View();
        }
    }
}