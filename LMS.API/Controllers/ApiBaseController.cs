using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace LMS.API.Controllers
{
    [EnableCors(origins: "http://localhost:58733", headers: "*", methods: "*")]
    public class ApiBaseController : ApiController
    {
        public int ClientId { get; set; }

        public ApiBaseController()
        {
            var identity = Thread.CurrentPrincipal.Identity;
        }       
    }
}
