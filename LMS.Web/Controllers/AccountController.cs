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
using LMS.Web.Extensions;
using System.Configuration;

namespace LMS.Web.Controllers
{
    public class AccountController : LMSBaseController
    {
        private Token token;

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, UserProfile user)
        {
            if (ModelState.IsValid)
            {
                token = GetToken(model);
                
                // check for a valid token
                if(token.access_token != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    // create a cookie based on the token
                    var tokenCookie = new HttpCookie("AvemtecLMS", token.access_token);
                    tokenCookie.Expires.AddDays(1);
                    HttpContext.Response.Cookies.Add(tokenCookie);

                    //initialise the user profile
                    this.Session.SetUserProfile(InitUserProfile(model.Username));

                    return RedirectToAction("Welcome", "Home");
                }

                ModelState.AddModelError("Username", "There was an error logging you in, please try again.");
                return View("../Home/Index", model);
            }

            ModelState.AddModelError("Username", "There was an error logging you in, please try again.");
            return View("../Home/Index", model);
        }

        private Token GetToken(LoginViewModel model)
        {
            HttpResponseMessage result;

            // make a POST request to the token generator 
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiBaseUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                var content = new FormUrlEncodedContent(new[] 
                {
                    new KeyValuePair<string, string>("userName", model.Username),
                    new KeyValuePair<string, string>("password", model.Password),
                    new KeyValuePair<string, string>("grant_type", "password")
                });

                result = client.PostAsync("/token", content).Result;
            }

            string resultContent = result.Content.ReadAsStringAsync().Result;
            Token token = JsonConvert.DeserializeObject<Token>(resultContent);

            return token;
        }

        private UserProfile InitUserProfile(string emailAddress)
        {
            HttpResponseMessage result;

            // make a POST request to the token generator 
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiBaseUrl"]);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new FormUrlEncodedContent(new[] 
                {
                    new KeyValuePair<string, string>("emailAddress", emailAddress)
                });

                result = client.PostAsync("/api/users/GetUserProfileByEmail", content).Result;
            }

            string resultContent = result.Content.ReadAsStringAsync().Result;
            UserProfile[] userProfiles = JsonConvert.DeserializeObject<UserProfile[]>(resultContent);
            return userProfiles[0];
        }
    }
}