// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebViewPageExtensions.cs" company="">
//   
// </copyright>
// <summary>
//   The web view page extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Extensions
{
    using System.Web.Mvc;

    /// <summary>
    /// The web view page extensions.
    /// </summary>
    public static class WebViewPageExtensions
    {
        /// <summary>
        /// The get api url.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetApiUrl(this WebViewPage page)
        {
            return page.ViewBag.ApiUrl;
        }

        /// <summary>
        /// The get token.
        /// </summary>
        /// <param name="page">
        /// The page.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetToken(this WebViewPage page)
        {
            return page.ViewBag.Token;
        }
    }
}