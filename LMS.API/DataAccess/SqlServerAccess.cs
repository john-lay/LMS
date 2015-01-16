namespace LMS.API.DataAccess
{
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    public class SqlServerAccess : BaseSqlAccess
    {
        #region Constructors
        public SqlServerAccess(string connectionString, string command, CommandType cmdType)
            : base(connectionString, command, cmdType)
        {
        }

        public SqlServerAccess(string command, CommandType cmdType)
            : base(command, cmdType)
        {
        }

        public SqlServerAccess(string connnectionString, string storedProcedureName)
            : base(connnectionString, storedProcedureName)
        {
        }

        public SqlServerAccess(string storedProcedureName)
            : base(storedProcedureName)
        {
        }

        public SqlServerAccess()
            : base(null)
        {
        }
        #endregion

        #region GetDbConnectionCommand
        protected override DbConnection GetDbConnection()
        {
            return new SqlConnection();
        }

        protected override DbCommand GetDbCommand()
        {
            return new SqlCommand();
        }
        #endregion

        #region GetDbParameter
        protected override DbParameter GetDbParameter(string name, object value)
        {
            name = AddAtCharIfNotExists(name);
            return new SqlParameter(name, value);
        }

        protected override DbParameter GetDbParameter(string name, DbType type, object value)
        {
            name = AddAtCharIfNotExists(name);
            return new SqlParameter(name, type) { Value = value };
        }

        protected override DbParameter GetDbParameter(string name, DbType type)
        {
            name = AddAtCharIfNotExists(name);
            return new SqlParameter(name, type);
        }

        protected override DbParameter GetDbParameter(string name, DbType type, ParameterDirection direction)
        {
            name = AddAtCharIfNotExists(name);
            return new SqlParameter(name, type) { Direction = direction };
        }
        #endregion

        #region Helper methods
        static string AddAtCharIfNotExists(string name)
        {
            return name[0] == '@' ? name : "@" + name;
        }
        #endregion

        protected override string GetDefaultConnectionStrig()
        {
            return GetConnectionStringByName("LMSContext");
        }
    }
}
