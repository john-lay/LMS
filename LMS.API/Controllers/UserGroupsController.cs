// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserGroupsController.cs" company="">
//   
// </copyright>
// <summary>
//   The user groups controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.Controllers
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using LMS.API.Contexts;
    using LMS.API.Models;

    /// <summary>
    /// The user groups controller.
    /// </summary>
    public class UserGroupsController : ApiBaseController
    {
        /// <summary>
        /// The db.
        /// </summary>
        private readonly LMSContext db = new LMSContext();

        /// <summary>
        /// The create group.
        /// </summary>
        /// <param name="group">
        /// The group.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        public async Task<IHttpActionResult> CreateGroup(UserGroup group)
        {
            group.ClientId = this.ClientId;

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            // generate group id
            group.UserGroupId = this.db.UserGroups.Count();

            // set parent id to root. i.e. -1
            group.ParentId = -1;

            this.db.UserGroups.Add(group);
            this.db.SaveChanges();

            return this.Ok(group);
        }

        /// <summary>
        /// The delete group.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteGroup(int id)
        {
            UserGroup group = this.db.UserGroups.Find(id);
            if (group == null)
            {
                return this.NotFound();
            }

            this.db.UserGroups.Remove(group);
            this.db.SaveChanges();

            return this.Ok(group);
        }

        /// <summary>
        /// The get group.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetGroup(int id)
        {
            // Group group = db.Groups.Find(id);
            // if (group == null)
            // {
            // return NotFound();
            // }

            // return Ok(group);
            UserGroup ret = this.db.UserGroups.Find(id);
            return ret == null ? this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "group not found") : this.Request.CreateResponse(HttpStatusCode.OK, ret);
        }

        /// <summary>
        /// The get groups.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<IQueryable<UserGroup>> GetGroups()
        {
            return this.db.UserGroups;
        }

        /// <summary>
        /// The update group.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="group">
        /// The group.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPut]
        public async Task<IHttpActionResult> UpdateGroup(int id, UserGroup group)
        {
            group.ClientId = this.ClientId;

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (id != group.UserGroupId)
            {
                return this.BadRequest();
            }

            this.db.Entry(group).State = EntityState.Modified;

            try
            {
                this.db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!this.GroupExists(id))
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
        /// The group exists.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool GroupExists(int id)
        {
            return this.db.UserGroups.Count(e => e.UserGroupId == id) > 0;
        }
    }
}