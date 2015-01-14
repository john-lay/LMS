using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LMS.API.Models;

namespace LMS.API.DataAccess
{
    public class ReportsDbAccess
    {
        public IEnumerable<Client> GetClients()
        {
            var sqlAccess = new SqlServerAccess("stp_getBasicReport");

            return sqlAccess.ToListByConvertFunctionFromReader(r =>
            {
                return new Client
                {
                    ClientId = r.To<int>("ClientId"),
                    Name = r.To<string>("Name"),
                    LogoTitle = r.To<string>("LogoTitle"),
                    LogoResource = r.To<string>("LogoResource")
                };
            });
        }
    }
}