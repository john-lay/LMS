// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportsDbAccess.cs" company="">
//   
// </copyright>
// <summary>
//   The reports db access.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using LMS.API.Models;

    /// <summary>
    /// The reports db access.
    /// </summary>
    public class ReportsDbAccess
    {
        /// <summary>
        /// The get basic report.
        /// </summary>
        /// <param name="clientId">
        /// The client id.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public async Task<IEnumerable<BasicReport>> GetBasicReport(int clientId)
        {
            var sqlAccess = new SqlServerAccess("stp_getBasicReport");
            sqlAccess.AddParameter("client_id", clientId);

            return await sqlAccess.ToListByConvertFunctionFromReader(r => new BasicReport
            {
                LearningComplete = r.To<bool>("LearningComplete"), 
                UserId = r.To<int>("UserId"), 
                FirstName = r.To<string>("FirstName"), 
                LastName = r.To<string>("LastName"), 
                EmailAddress = r.To<string>("EmailAddress"), 
                UserGroupName = r.To<string>("UserGroupName"), 
                StartDate = r.To<DateTime>("StartDate"), 
                EndDate = r.To<DateTime>("EndDate"), 
                IsRolling = r.To<bool>("IsRolling"), 
                CourseName = r.To<string>("CourseName"), 
                CourseType = r.To<string>("CourseType"), 
                CourseCategoryName = r.To<string>("CourseCategoryName")
            });
        }
    }
}