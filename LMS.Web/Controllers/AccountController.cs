// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="">
//   
// </copyright>
// <summary>
//   The account controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    using LMS.Web.Extensions;
    using LMS.Web.Infrastructure;
    using LMS.Web.Models;

    using Newtonsoft.Json;

    /// <summary>
    /// The account controller.
    /// </summary>
    public class AccountController : LmsBaseController
    {
        /// <summary>
        /// The token.
        /// </summary>
        private Token token;

        /// <summary>
        /// The login.
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
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, UserProfile user)
        {
            if (this.ModelState.IsValid)
            {
                this.token = await this.GetToken(model);

                // check for a valid token
                if (this.token.access_token != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);

                    // create a cookie based on the token
                    var tokenCookie = new HttpCookie("AvemtecLMS", this.token.access_token);
                    tokenCookie.Expires.AddDays(1);
                    this.HttpContext.Response.Cookies.Add(tokenCookie);

                    // initialise the user profile
                    this.Session.SetUserProfile(await this.InitializeUserProfile(model.Username));

                    return this.RedirectToAction("Welcome", "Home");
                }

                this.ModelState.AddModelError("Username", "There was an error logging you in, please try again.");
                return this.View("../Home/Index", model);
            }

            this.ModelState.AddModelError("Username", "There was an error logging you in, please try again.");
            return this.View("../Home/Index", model);
        }

        /// <summary>
        /// The get token.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<Token> GetToken(LoginViewModel model)
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

                result = await client.PostAsync("/token", content);
            }

            string resultContent = result.Content.ReadAsStringAsync().Result;
            var clientToken = JsonConvert.DeserializeObject<Token>(resultContent);

            return clientToken;
        }

        /// <summary>
        /// The initialize user profile.
        /// </summary>
        /// <param name="emailAddress">
        /// The email address.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task<UserProfile> InitializeUserProfile(string emailAddress)
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

                result = await client.PostAsync("/api/users/GetUserProfileByEmail", content);
            }

            string resultContent = await result.Content.ReadAsStringAsync();
            var userProfiles = JsonConvert.DeserializeObject<UserProfile[]>(resultContent);
            return userProfiles[0];
        }
    }
}