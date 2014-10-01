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
                    // add the token to the session, and initialize the Api base URL
                    SetTokenForApplication(token);
                    return RedirectToAction("Index", "Client");
                }
            }

            ModelState.AddModelError(string.Empty, "There was an error logging you in, please try again.");
            return View("../Home/Index", model);
        }

        private void SetTokenForApplication(Token token)
        {
            HttpContext.Session.SetUserToken(token.access_token);
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
    }
}