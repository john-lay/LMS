// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CourseController.cs" company="">
//   
// </copyright>
// <summary>
//   The course controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    using LMS.Web.Infrastructure;
    using LMS.Web.Models;

    /// <summary>
    /// The course controller.
    /// </summary>
    public class CourseController : LmsBaseController
    {
        /// <summary>
        /// The content.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult> Content(CourseViewModel model, UserProfile user)
        {
            if (this.ModelState.IsValid)
            {
                HttpServerUtility server = System.Web.HttpContext.Current.Server;
                string clientId = user.ClientId.ToString(CultureInfo.InvariantCulture);
                string courseId = model.CourseId.ToString(CultureInfo.InvariantCulture);

                // check folders exist - UPLOADS FOLDER
                if (!Directory.Exists(server.MapPath("~/uploads")))
                {
                    Directory.CreateDirectory(server.MapPath("~/uploads"));
                }

                // CLIENT FOLDER
                if (!Directory.Exists(server.MapPath("~/uploads/" + clientId)))
                {
                    Directory.CreateDirectory(server.MapPath("~/uploads/" + clientId));
                }

                // COURSE FOLDER
                if (!Directory.Exists(server.MapPath("~/uploads/" + clientId + "/" + courseId)))
                {
                    Directory.CreateDirectory(server.MapPath("~/uploads/" + "/" + clientId + "/" + courseId));
                }

                string targetFolder = server.MapPath("~/uploads/" + clientId + "/" + courseId + "/");
                string targetPath = Path.Combine(targetFolder, model.Content.FileName);
                string linkToFile = "/uploads/" + clientId + "/" + courseId + "/" + model.Content.FileName;

                // save file to disk
                model.Content.SaveAs(targetPath);

                // update record
                bool fileUploadSuccess = await this.AddContentToDb(model.Content.FileName, linkToFile, courseId);

                if (fileUploadSuccess)
                {
                    this.ViewBag.AlertStatus = "success";
                    this.ViewBag.AlertMessage = "File: " + model.Content.FileName + " successfully uploaded.";
                }
                else
                {
                    this.ViewBag.AlertStatus = "danger";
                    this.ViewBag.AlertMessage = "File: " + model.Content.FileName + " could not be saved.";
                }
            }
            else
            {
                this.ViewBag.AlertStatus = "danger";
                this.ViewBag.AlertMessage = "Please check the form for errors and try again.";
            }

            model.CourseTypeList = await this.GetCourseTypeList();

            return this.View("Edit", model);
        }

        /// <summary>
        /// The edit.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ActionResult> Edit(int id)
        {
            var model = new CourseViewModel
                            {
                                CourseId = id, CourseTypeList = await this.GetCourseTypeList()
                            };

            return this.View(model);
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

        /// <summary>
        /// The select course.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ActionResult> SelectCourse()
        {
            return this.View();
        }

        /// <summary>
        /// The session.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ActionResult> Session(int id)
        {
            var model = new CourseViewModel
                            {
                                CourseId = id
                            };

            return this.View(model);
        }

        /// <summary>
        /// The add content to db.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <param name="courseId">
        /// The course id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<bool> AddContentToDb(string fileName, string filePath, string courseId)
        {
            HttpResponseMessage result;

            // make a POST request to the contents controller
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiBaseUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                // create an api content object
                var content = new FormUrlEncodedContent(new[]
                                                            {
                                                                new KeyValuePair<string, string>("Name", fileName), 
                                                                new KeyValuePair<string, string>("Resource", filePath), 
                                                                new KeyValuePair<string, string>("CourseId", courseId)
                                                            });

                result = client.PostAsync("/api/Contents/CreateContent/", content).Result;
            }

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// The get course type list.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<IEnumerable<SelectListItem>> GetCourseTypeList()
        {
            return new List<SelectListItem>
                       {
                           new SelectListItem
                               {
                                   Value = string.Empty, Text = "Please select the type..."
                               }, 
                           new SelectListItem
                               {
                                   Value = "rtb", Text = "Read Tick Box"
                               }, 
                           
                           // new SelectListItem { Value = "elearning", Text = "E-Learning" },
                       };
        }
    }
}