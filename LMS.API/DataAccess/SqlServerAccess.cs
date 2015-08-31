// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SqlServerAccess.cs" company="">
//   
// </copyright>
// <summary>
//   The sql server access.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace LMS.API.DataAccess
{
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    /// <summary>
    ///     The sql server access.
    /// </summary>
    public class SqlServerAccess : BaseSqlAccess
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerAccess"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="cmdType">
        /// The cmd type.
        /// </param>
        public SqlServerAccess(string connectionString, string command, CommandType cmdType)
            : base(connectionString, command, cmdType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerAccess"/> class.
        /// </summary>
        /// <param name="command">
        /// The command.
        /// </param>
        /// <param name="cmdType">
        /// The cmd type.
        /// </param>
        public SqlServerAccess(string command, CommandType cmdType)
            : base(command, cmdType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerAccess"/> class.
        /// </summary>
        /// <param name="connnectionString">
        /// The connnection string.
        /// </param>
        /// <param name="storedProcedureName">
        /// The stored procedure name.
        /// </param>
        public SqlServerAccess(string connnectionString, string storedProcedureName)
            : base(connnectionString, storedProcedureName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerAccess"/> class.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The stored procedure name.
        /// </param>
        public SqlServerAccess(string storedProcedureName)
            : base(storedProcedureName)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SqlServerAccess" /> class.
        /// </summary>
        public SqlServerAccess()
            : base(null)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The get db command.
        /// </summary>
        /// <returns>
        ///     The <see cref="DbCommand" />.
        /// </returns>
        protected override DbCommand GetDbCommand()
        {
            return new SqlCommand();
        }

        /// <summary>
        ///     The get db connection.
        /// </summary>
        /// <returns>
        ///     The <see cref="DbConnection" />.
        /// </returns>
        protected override DbConnection GetDbConnection()
        {
            return new SqlConnection();
        }

        /// <summary>
        /// The get db parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="DbParameter"/>.
        /// </returns>
        protected override DbParameter GetDbParameter(string name, object value)
        {
            name = AddAtCharIfNotExists(name);
            return new SqlParameter(name, value);
        }

        /// <summary>
        /// The get db parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="DbParameter"/>.
        /// </returns>
        protected override DbParameter GetDbParameter(string name, DbType type, object value)
        {
            name = AddAtCharIfNotExists(name);
            return new SqlParameter(name, type)
                       {
                           Value = value
                       };
        }

        /// <summary>
        /// The get db parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="DbParameter"/>.
        /// </returns>
        protected override DbParameter GetDbParameter(string name, DbType type)
        {
            name = AddAtCharIfNotExists(name);
            return new SqlParameter(name, type);
        }

        /// <summary>
        /// The get db parameter.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="direction">
        /// The direction.
        /// </param>
        /// <returns>
        /// The <see cref="DbParameter"/>.
        /// </returns>
        protected override DbParameter GetDbParameter(string name, DbType type, ParameterDirection direction)
        {
            name = AddAtCharIfNotExists(name);
            return new SqlParameter(name, type)
                       {
                           Direction = direction
                       };
        }

        /// <summary>
        ///     The get default connection string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        protected override string GetDefaultConnectionString()
        {
            return this.GetConnectionStringByName("LMSContext");
        }

        /// <summary>
        /// The add at char if not exists.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string AddAtCharIfNotExists(string name)
        {
            return name[0] == '@' ? name : "@" + name;
        }

        #endregion
    }
}