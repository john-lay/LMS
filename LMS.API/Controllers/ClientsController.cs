// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientsController.cs" company="">
//   
// </copyright>
// <summary>
//   The clients controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Controllers
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    using LMS.API.Contexts;
    using LMS.API.Infrastructure;
    using LMS.API.Models;

    /// <summary>
    /// The clients controller.
    /// </summary>
    public class ClientsController : ApiBaseController
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LMSContext db = new LMSContext();

        /// <summary>
        /// The create client.
        /// </summary>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [LMSAuthorize(Role = "SuperAdmin")]
        public async Task<IHttpActionResult> CreateClient([FromBody] Client client)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            this.db.Clients.Add(client);
            this.db.SaveChanges();

            return this.Ok(client);
        }

        // [HttpPost]
        // public HttpResponseMessage UploadLogo()
        // {
        // HttpResponseMessage result = null;
        // var httpRequest = HttpContext.Current.Request;

        // if (httpRequest.Files.Count > 0)
        // {
        // var docfiles = new List<string>();
        // foreach (string file in httpRequest.Files)
        // {
        // var postedFile = httpRequest.Files[file];
        // var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
        // postedFile.SaveAs(filePath);

        // docfiles.Add(filePath);
        // }
        // result = Request.CreateResponse(HttpStatusCode.Created, docfiles);
        // }
        // else
        // {
        // result = Request.CreateResponse(HttpStatusCode.BadRequest);
        // }

        // //httpRequest.Form["Name"]

        // //db.Clients.Add(client);
        // //db.SaveChanges();

        // return result;
        // }

        /// <summary>
        /// The delete client.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteClient(int id)
        {
            Client client = this.db.Clients.Find(id);
            if (client == null)
            {
                return this.NotFound();
            }

            this.db.Clients.Remove(client);
            this.db.SaveChanges();

            return this.Ok(client);
        }

        /// <summary>
        /// The get client.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        public async Task<IHttpActionResult> GetClient(int id)
        {
            Client client = this.db.Clients.Find(id);
            if (client == null)
            {
                return this.NotFound();
            }

            return this.Ok(client);
        }

        /// <summary>
        /// The get clients.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        public async Task<IQueryable<Client>> GetClients()
        {
            return this.db.Clients;
        }

        /// <summary>
        /// The update client.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="client">
        /// The client.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        public async Task<IHttpActionResult> UpdateClient(int id, [FromBody] Client client)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != client.ClientId)
            {
                return this.BadRequest();
            }

            this.db.Entry(client).State = EntityState.Modified;

            try
            {
                this.db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.ClientExists(id))
                {
                    return this.NotFound();
                }

                throw;
            }

            return this.StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// The client exists.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private bool ClientExists(int id)
        {
            return this.db.Clients.Count(e => e.ClientId == id) > 0;
        }
    }
}