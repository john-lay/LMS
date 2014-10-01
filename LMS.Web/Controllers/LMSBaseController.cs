using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Web.Extensions;

namespace LMS.Web.Controllers
{
    public class LMSBaseController : Controller
    {
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            InitResources();
        }

        private void InitResources()
        {
            ViewBag.Token = HttpContext.Session.GetUserToken();
            ViewBag.ApiUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
        }
    }
}