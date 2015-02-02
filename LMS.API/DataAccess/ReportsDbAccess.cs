using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LMS.API.Models;

namespace LMS.API.DataAccess
{
    public class ReportsDbAccess
    {
        public IEnumerable<BasicReport> GetBasicReport(int clientId)
        {
            var sqlAccess = new SqlServerAccess("stp_getBasicReport");
            sqlAccess.AddParameter("client_id", clientId);

            return sqlAccess.ToListByConvertFunctionFromReader(r => new BasicReport
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