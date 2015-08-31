// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="">
//   
// </copyright>
// <summary>
//   The account controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    using LMS.API.Auth;
    using LMS.API.Models;

    using Microsoft.AspNet.Identity;

    /// <summary>
    /// The account controller.
    /// </summary>
    [RoutePrefix("api/Account")]
    public class AccountController : ApiBaseController
    {
        /// <summary>
        /// The _repo.
        /// </summary>
        private readonly AuthRepository repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        public AccountController()
        {
            this.repo = new AuthRepository();
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="userModel">
        /// The user model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(User userModel)
        {
            userModel.ClientId = this.ClientId;

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            IdentityResult result = await this.repo.RegisterUser(userModel);

            IHttpActionResult errorResult = await this.GetErrorResult(result);

            if (errorResult != null)
            {
                throw new Exception(result.Errors.ElementAt(0));
            }

            return this.Ok();
        }

        /// <summary>
        /// The register admin.
        /// </summary>
        /// <param name="userModel">
        /// The user model.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        [AllowAnonymous]
        [Route("RegisterAdmin")]
        public async Task<IHttpActionResult> RegisterAdmin(User userModel)
        {
            // client id comes from the web application
            // userModel.ClientId = this.ClientId;
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            IdentityResult result = await this.repo.RegisterAdmin(userModel);

            IHttpActionResult errorResult = await this.GetErrorResult(result);

            if (errorResult != null)
            {
                throw new Exception(result.Errors.ElementAt(0));
            }

            return this.Ok();
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.repo.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// The get error result.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <returns>
        /// The <see cref="IHttpActionResult"/>.
        /// </returns>
        private async Task<IHttpActionResult> GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (this.ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return this.BadRequest();
                }

                return this.BadRequest(this.ModelState);
            }

            return null;
        }
    }
}