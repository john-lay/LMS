// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiBaseController.cs" company="">
//   
// </copyright>
// <summary>
//   The api base controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Cors;
    
    /// <summary>
    /// The api base controller.
    /// </summary>
    // [EnableCors(origins: "http://localhost:58733", headers: "*", methods: "*")]
    // [EnableCors(origins: "http://www.avemtec.somee.com/", headers: "*", methods: "*")]
    // [EnableCors(origins: "http://avemtec.azurewebsites.net/", headers: "*", methods: "*")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApiBaseController : ApiController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiBaseController"/> class.
        /// </summary>
        public ApiBaseController()
        {
            this.SetClientId();

            // var identity = Thread.CurrentPrincipal.Identity;
        }

        /// <summary>
        /// Gets or sets the client id.
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// The set client id.
        /// </summary>
        private void SetClientId()
        {
            this.ClientId = -1;

            // try and grab the client Id from the users claim
            IIdentity user = HttpContext.Current.User.Identity;

            if (user.IsAuthenticated)
            {
                var claimsidentity = (ClaimsIdentity)user;
                IEnumerable<Claim> claims = claimsidentity.Claims;
                string clientId = claims.First(c => c.Type == "clientId").Value;

                this.ClientId = Convert.ToInt32(clientId);
            }
        }
    }
}