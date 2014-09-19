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
    public class ClientController : Controller
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

        //[HttpPost]
        //public async Task<ActionResult> Manage(ClientViewModel model, UserProfile user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        using (var client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri("http://localhost:58021/");
        //            client.DefaultRequestHeaders.Accept.Clear();
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //            // New code:
        //            HttpResponseMessage response = await client.PostAsync("api/clients/CreateClient", JsonConvert.SerializeObject(model));
        //            if (response.IsSuccessStatusCode)
        //            {
        //                return View();
        //            }
        //        }
        //    }

        //    return View();
        //}

        public ActionResult Logo()
        {
            var model = new UploadViewModel();
            return View(model);
        }

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