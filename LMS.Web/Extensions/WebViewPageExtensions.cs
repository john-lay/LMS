using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Web.Extensions
{
    public static class WebViewPageExtensions
    {
        public static string GetApiUrl(this WebViewPage page)
        {
            return page.ViewBag.ApiUrl;
        }

        public static string GetToken(this WebViewPage page)
        {
            return page.ViewBag.Token;
        }
    }
}