using LMS.Web.Infrastructure;
using LMS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LMS.Web.Controllers
{
    public class ClientController : LMSBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Manage()
        {
            var model = new ClientViewModel();
            return View(model);
        }

        //public ActionResult Logo()
        //{
        //    var model = new UploadViewModel();
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Logo(UploadViewModel model, UserProfile user)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        // save file to disk
        //        string path = @"C:\Temp\";

        //        if (model.File != null)
        //            model.File.SaveAs(path + model.File.FileName);

        //        // update record
        //        HttpClient client = new HttpClient();
        //        var task = client.PostAsJsonAsync<UploadViewModel>("http://localhost:<port>/api/CustomerDetail", model);
        //    }
        //    return View();
        //}
    }
}