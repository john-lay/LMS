// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportsController.cs" company="">
//   
// </copyright>
// <summary>
//   The reports controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LMS.API.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Script.Serialization;

    using LMS.API.DataAccess;

    /// <summary>
    ///     The reports controller.
    /// </summary>
    public class ReportsController : ApiBaseController
    {
        // GET: api/Clients
        // http://localhost:58021/api/Reports/GetBasicReport
        /// <summary>
        ///     The get basic report.
        /// </summary>
        /// <returns>
        ///     The <see cref="Task" />.
        /// </returns>
        [HttpGet]
        public async Task<string> GetBasicReport()
        {
            var repo = new ReportsDbAccess();

            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(await repo.GetBasicReport(this.ClientId));
        }
    }
}