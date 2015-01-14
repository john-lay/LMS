using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LMS.API.DataAccess;
using LMS.API.Models;

namespace LMS.API.Controllers
{
    public class ReportsController : ApiBaseController
    {
        // GET: api/Clients
        [HttpGet]
        public void GetBasicReport()
        {
            var test = new ReportsDbAccess();

            var t = test.GetClients();
            var i = 1;
            //http://localhost:58021/api/Reports/GetBasicReport
        }
    }
}
