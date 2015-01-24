using LMS.Web.Infrastructure;
using LMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;

namespace LMS.Web.Controllers
{
    public class CourseController : LMSBaseController
    {
        public ActionResult Manage()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var model = new CourseViewModel 
            {
                CourseId = id,
                CourseTypeList = GetCourseTypeList()
            };

            return View(model);
        }

        public ActionResult SelectCourse()
        {
            return View();
        }

        public ActionResult Session(int id)
        {
            var model = new CourseViewModel
            {
                CourseId = id
            };

            return View(model);
        }

        private IEnumerable<SelectListItem> GetCourseTypeList()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = string.Empty, Text = "Please select the type..." },
                new SelectListItem { Value = "rtb", Text = "Read Tick Box" },
                //new SelectListItem { Value = "elearning", Text = "E-Learning" },
            };
        }

        [HttpPost]
        public ActionResult Content(CourseViewModel model, UserProfile user)
        {
            bool fileUploadSuccess = false;

            if (ModelState.IsValid)
            {
                var server = System.Web.HttpContext.Current.Server;
                string clientId = user.ClientId.ToString();
                string courseId = model.CourseId.ToString();

                // check folders exist - UPLOADS FOLDER
                if (!Directory.Exists(server.MapPath("~/uploads"))) Directory.CreateDirectory(server.MapPath("~/uploads"));

                // CLIENT FOLDER
                if (!Directory.Exists(server.MapPath("~/uploads/" + clientId))) Directory.CreateDirectory(server.MapPath("~/uploads/" + clientId));

                // COURSE FOLDER
                if (!Directory.Exists(server.MapPath("~/uploads/" + clientId + "/" + courseId))) Directory.CreateDirectory(server.MapPath("~/uploads/" + "/" + clientId + "/" + courseId));

                string targetFolder = server.MapPath("~/uploads/" + clientId + "/" + courseId + "/");
                string targetPath = Path.Combine(targetFolder, model.Content.FileName);
                string linkToFile = "/uploads/" + clientId + "/" + courseId + "/" + model.Content.FileName;

                // save file to disk
                model.Content.SaveAs(targetPath);

                // update record
                fileUploadSuccess = AddContentToDb(model.Content.FileName, linkToFile, courseId);

                if (fileUploadSuccess)
                {
                    ViewBag.AlertStatus = "success";
                    ViewBag.AlertMessage = "File: " + model.Content.FileName + " successfully uploaded.";
                }
                else
                {
                    ViewBag.AlertStatus = "danger";
                    ViewBag.AlertMessage = "File: " + model.Content.FileName + " could not be saved.";
                }
            }
            else
            {
                ViewBag.AlertStatus = "danger";
                ViewBag.AlertMessage = "Please check the form for errors and try again.";
            }

            model.CourseTypeList = GetCourseTypeList();

            return View("Edit", model);
        }

        private bool AddContentToDb(string fileName, string filePath, string courseId)
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
                return true;

            return false;
        }
    }
}