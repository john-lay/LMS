// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientController.cs" company="">
//   
// </copyright>
// <summary>
//   The client controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using LMS.Web.Models;

    /// <summary>
    /// The client controller.
    /// </summary>
    public class ClientController : LmsBaseController
    {
        /// <summary>
        /// The manage.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ActionResult> Manage()
        {
            var model = new ClientViewModel();
            return this.View(model);
        }

        // public ActionResult Logo()
        // {
        // var model = new UploadViewModel();
        // return View(model);
        // }

        // [HttpPost]
        // public ActionResult Logo(UploadViewModel model, UserProfile user)
        // {
        // if (ModelState.IsValid)
        // {

        // // save file to disk
        // string path = @"C:\Temp\";

        // if (model.File != null)
        // model.File.SaveAs(path + model.File.FileName);

        // // update record
        // HttpClient client = new HttpClient();
        // var task = client.PostAsJsonAsync<UploadViewModel>("http://localhost:<port>/api/CustomerDetail", model);
        // }
        // return View();
        // }
    }
}