// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportController.cs" company="">
//   
// </copyright>
// <summary>
//   The report controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    /// <summary>
    /// The report controller.
    /// </summary>
    public class ReportController : LmsBaseController
    {
        /// <summary>
        /// The index.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ActionResult> Index()
        {
            return this.View();
        }
    }
}