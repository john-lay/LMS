// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApiConfig.cs" company="">
//   
// </copyright>
// <summary>
//   The web api config.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API
{
    using System.Web.Http;

    /// <summary>
    /// The web api config.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="config">
        /// The config.
        /// </param>
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApiWithId", "api/{controller}/{id}",
                new { id = RouteParameter.Optional }, new { id = @"\d+" });

            config.Routes.MapHttpRoute("DefaultApiWithAction", "api/{controller}/{action}");

            config.Routes.MapHttpRoute("DefaultApiWithActionAndId", "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional }, new { id = @"\d+" });

            // var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            // jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }
    }
}