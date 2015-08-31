// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LMSBaseController.cs" company="">
//   
// </copyright>
// <summary>
//   The lms base controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Controllers
{
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// The lms base controller.
    /// </summary>
    [Authorize]
    public class LmsBaseController : Controller
    {
        /// <summary>
        /// The on result executing.
        /// </summary>
        /// <param name="filterContext">
        /// The filter context.
        /// </param>
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            this.InitResources();
        }

        /// <summary>
        /// The init resources.
        /// </summary>
        private void InitResources()
        {
            // ViewBag.Token = HttpContext.Session.GetUserToken();
            HttpCookie tokenCookie = this.Request.Cookies["AvemtecLMS"];
            if (tokenCookie != null)
            {
                this.ViewBag.Token = tokenCookie.Value;
            }

            this.ViewBag.ApiUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
        }
    }
}