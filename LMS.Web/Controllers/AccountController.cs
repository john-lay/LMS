using LMS.Web.Infrastructure;
using LMS.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LMS.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, UserProfile user)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage result;

                // make a POST request to the token generator 
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:58021/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                    var content = new FormUrlEncodedContent(new[] 
                    {
                        new KeyValuePair<string, string>("userName", model.Username),
                        new KeyValuePair<string, string>("password", model.Password),
                        new KeyValuePair<string, string>("grant_type", "password")
                    });

                    result = client.PostAsync("token", content).Result;                    
                }

                string resultContent = result.Content.ReadAsStringAsync().Result;
                Token token = JsonConvert.DeserializeObject<Token>(resultContent);

                // check for a valid token
                if(token.access_token != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    // add the token to the session object
                    user.Token = token.access_token;
                    return RedirectToAction("Index", "Client");
                }
            }

            ModelState.AddModelError(string.Empty, "There was an error logging you in, please try again.");
            return View("../Home/Index", model);
        }
    }
}