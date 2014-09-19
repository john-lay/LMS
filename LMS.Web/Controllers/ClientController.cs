﻿using LMS.Web.Infrastructure;
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

        public ActionResult Create()
        {
            var model = new ClientViewModel();
            return View(model);
        }

        //[HttpPost]
        //public async Task<ActionResult> Create(ClientViewModel model, UserProfile user)
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
    }
}