using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LMS.API.Controllers
{
    //[EnableCors(origins: "http://localhost:58733", headers: "*", methods: "*")]
    [EnableCors(origins: "http://www.avemtec.somee.com/", headers: "*", methods: "*")]
    public class ApiBaseController : ApiController
    {
        public int ClientId { get; set; }

        public ApiBaseController()
        {
            SetClientId();
            //var identity = Thread.CurrentPrincipal.Identity;
        }

        private void SetClientId()
        {
            this.ClientId = -1;

            // try and grab the client Id from the users claim
            var user = HttpContext.Current.User.Identity;

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
