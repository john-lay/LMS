using System.Linq;
using System.Web.Http;
using LMS.API.DataAccess;

namespace LMS.API.Controllers
{
    using System.Web.Script.Serialization;

    public class ReportsController : ApiBaseController
    {
        // GET: api/Clients
        // http://localhost:58021/api/Reports/GetBasicReport
        [HttpGet]
        public string GetBasicReport()
        {
            var repo = new ReportsDbAccess();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(repo.GetBasicReport(this.ClientId).ToArray());
        }
    }
}
