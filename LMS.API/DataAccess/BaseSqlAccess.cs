// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseSqlAccess.cs" company="">
//   
// </copyright>
// <summary>
//   The base sql access.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.API.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;
    using System.Threading.Tasks;

    /// <summary>
    /// The base sql access.
    /// </summary>
    public abstract class BaseSqlAccess
    {
        #region Fields

        /// <summary>
        /// The transaction.
        /// </summary>
        protected DbTransaction transaction = null;

        /// <summary>
        /// The command type.
        /// </summary>
        private readonly CommandType commandType = CommandType.Text;

        /// <summary>
        /// The connection string.
        /// </summary>
        private string connectionString;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSqlAccess"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="sqlCommand">
        /// The sql command.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        public BaseSqlAccess(string connectionString, string sqlCommand, CommandType commandType)
        {
            this.commandType = commandType;

            this.connectionString = connectionString;
            this.Command = this.GetDbCommand();
            this.Command.CommandText = sqlCommand;
            this.Command.CommandType = commandType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSqlAccess"/> class.
        /// </summary>
        /// <param name="sqlCommand">
        /// The sql command.
        /// </param>
        /// <param name="commandType">
        /// The command type.
        /// </param>
        public BaseSqlAccess(string sqlCommand, CommandType commandType)
            : this(null, sqlCommand, commandType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSqlAccess"/> class.
        /// </summary>
        /// <param name="connectionString">
        /// The connection string.
        /// </param>
        /// <param name="storedProcedureName">
        /// The stored procedure name.
        /// </param>
        public BaseSqlAccess(string connectionString, string storedProcedureName)
            : this(connectionString, storedProcedureName, CommandType.StoredProcedure)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSqlAccess"/> class.
        /// </summary>
        /// <param name="storedProcedureName">
        /// The stored procedure name.
        /// </param>
        public BaseSqlAccess(string storedProcedureName)
            : this(null, storedProcedureName, CommandType.StoredProcedure)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the default connection string.
        /// </summary>
        public string DefaultConnectionString
        {
            get
            {
                return this.GetDefaultConnectionString();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the command.
        /// </summary>
        protected DbCommand Command { get; private set; }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        protected DbConnection Connection { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Executes actions that use SQL access in the same transaction. You need to call ResetCommand and
        ///     Excute...InOpenedTransaction inside the actions
        /// </summary>
        /// <param name="actions">
        /// Actions to execute inside the same SQL transaction
        /// </param>
        public static void ExecuteInOpenedTransaction(params Action<BaseSqlAccess>[] actions)
        {
            var sqlAccess = new SqlServerAccess();
            try
            {
                sqlAccess.OpenConnection(true);

                foreach (var action in actions)
                {
                    action(sqlAccess);
                }

                sqlAccess.CommitTransaction();
            }
            catch
            {
                sqlAccess.RollbackTransaction();
                throw;
            }
            finally
            {
                sqlAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Add a parameter to parameters collection of command. If value is null or is String.Empty sets the value to
        ///     DBNull.Value
        /// </summary>
        /// <param name="name">
        /// Name of parameter
        /// </param>
        /// <param name="value">
        /// Value of parameter
        /// </param>
        public void AddParameter(string name, object value)
        {
            this.SetValueToDbNullIfNull(ref value);

            this.Command.Parameters.Add(this.GetDbParameter(name, value));
        }

        /// <summary>
        /// Add a parameter to parameters collection of command. If value is null or is String.Empty sets the value to
        ///     DBNull.Value
        /// </summary>
        /// <param name="name">
        /// Name of parameter
        /// </param>
        /// <param name="type">
        /// DbType of parameter
        /// </param>
        /// <param name="value">
        /// Value of parameter
        /// </param>
        public void AddParameter(string name, DbType type, object value)
        {
            this.SetValueToDbNullIfNull(ref value);
            this.Command.Parameters.Add(GetDbParameter(name, type, value));
        }

        /// <summary>
        /// The add parameter.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// The <see cref="DbParameter"/>.
        /// </returns>
        public DbParameter AddParameter(DbParameter parameter)
        {
            this.Command.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// The add str param and set to db null if null or empty.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public void AddStrParamAndSetToDbNullIfNullOrEmpty(string name, string value)
        {
            this.AddParameter(name, string.IsNullOrEmpty(value) ? null : value);
        }

        /// <summary>
        /// The close connection.
        /// </summary>
        public void CloseConnection()
        {
            if (this.Connection != null && this.Connection.State != ConnectionState.Closed)
            {
                this.Connection.Close();
            }
        }

        /// <summary>
        /// The commit transaction.
        /// </summary>
        public void CommitTransaction()
        {
            if (this.transaction != null)
            {
                this.transaction.Commit();
            }
        }

        /// <summary>
        /// The execute non query.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public async Task<int> ExecuteNonQuery()
        {
            using (this.GetOpenConnectionAfterAssignToCommand())
            {
                return await this.Command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// The execute non query in opened transaction.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public async Task<int> ExecuteNonQueryInOpenedTransaction()
        {
            this.SetConnectionAndTransaction();

            return await this.Command.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// The execute reader.
        /// </summary>
        /// <param name="actionsWhenRead">
        /// The actions when read.
        /// </param>
        public async Task ExecuteReader(params Action<DbDataReader>[] actionsWhenRead)
        {
            using (this.GetOpenConnectionAfterAssignToCommand())
            {
                await this.ReadDataFromDbDataReader(actionsWhenRead);
            }
        }

        /// <summary>
        /// The execute reader first row if exists.
        /// </summary>
        /// <param name="functionWhenRead">
        /// The function when read.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public async Task<T> ExecuteReaderFirstRowIfExists<T>(Func<DbDataReader, T> functionWhenRead)
        {
            using (this.GetOpenConnectionAfterAssignToCommand())
            {
                using (DbDataReader reader = await this.Command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        return functionWhenRead(reader);
                    }

                    return default(T);
                }
            }
        }

        /// <summary>
        /// The execute reader in opened transaction.
        /// </summary>
        /// <param name="actionsWhenRead">
        /// The actions when read.
        /// </param>
        public void ExecuteReaderInOpenedTransaction(params Action<DbDataReader>[] actionsWhenRead)
        {
            this.SetConnectionAndTransaction();

            this.ReadDataFromDbDataReader(actionsWhenRead);
        }

        /// <summary>
        /// The execute scalar.
        /// </summary>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public async Task<object> ExecuteScalar()
        {
            using (this.GetOpenConnectionAfterAssignToCommand())
            {
                return await this.Command.ExecuteScalarAsync();
            }
        }

        /// <summary>
        /// The open connection.
        /// </summary>
        /// <param name="openTransaction">
        /// The open transaction.
        /// </param>
        public void OpenConnection(bool openTransaction)
        {
            this.Connection = this.GetDbConnection();
            if (string.IsNullOrEmpty(this.connectionString))
            {
                this.connectionString = this.DefaultConnectionString;
            }

            this.Connection.ConnectionString = this.connectionString;
            this.Connection.Open();

            if (openTransaction)
            {
                this.transaction = this.Connection.BeginTransaction();
            }
        }

        /// <summary>
        /// The reset command.
        /// </summary>
        /// <param name="commandText">
        /// The command text.
        /// </param>
        public void ResetCommand(string commandText)
        {
            this.Command = this.GetDbCommand();
            this.Command.CommandText = commandText;
            this.Command.CommandType = this.commandType;
        }

        /// <summary>
        /// The rollback transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            if (this.transaction != null)
            {
                this.transaction.Rollback();
            }
        }

        /// <summary>
        /// The set command timeout.
        /// </summary>
        /// <param name="timeout">
        /// The timeout.
        /// </param>
        public void SetCommandTimeout(int timeout)
        {
            this.Command.CommandTimeout = timeout;
        }

        /// <summary>
        /// The to list by convert function from reader.
        /// </summary>
        /// <param name="fromReaderToTypeT">
        /// The from reader to type t.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public async Task<List<T>> ToListByConvertFunctionFromReader<T>(Func<DbDataReader, T> fromReaderToTypeT)
        {
            var listOfEntities = new List<T>();
            await this.ExecuteReader(reader =>
                                   {
                                       T entity = fromReaderToTypeT(reader);
                                       listOfEntities.Add(entity);
                                   });
            return listOfEntities;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get connection string by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected string GetConnectionStringByName(string name)
        {
            ConnectionStringSettings connString = ConfigurationManager.ConnectionStrings[name];
            if (connString == null)
            {
                return null;
            }

            return connString.ConnectionString;
        }

        /// <summary>
        /// The get db command.
        /// </summary>
        /// <returns>
        /// The <see cref="DbCommand"/>.
        /// </returns>
        protected abstract DbCommand GetDbCommand();

        /// <summary>
        /// The get db connection.
        /// </summary>
        /// <returns>
        /// The <see cref="DbConnection"/>.
        /// </returns>
        protected abstract DbConnection GetDbConnection();

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
        protected abstract DbParameter GetDbParameter(string name, object value);

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
        protected abstract DbParameter GetDbParameter(string name, DbType type, object value);

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
        protected abstract DbParameter GetDbParameter(string name, DbType type);

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
        protected abstract DbParameter GetDbParameter(string name, DbType type, ParameterDirection direction);

        /// <summary>
        /// The get default connection strig.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        protected abstract string GetDefaultConnectionString();

        /// <summary>
        /// The get open connection after assign to command.
        /// </summary>
        /// <returns>
        /// The <see cref="DbConnection"/>.
        /// </returns>
        protected DbConnection GetOpenConnectionAfterAssignToCommand()
        {
            this.OpenConnection(false);

            this.Command.Connection = this.Connection;
            return this.Connection;
        }

        /// <summary>
        /// The read data from db data reader.
        /// </summary>
        /// <param name="actionsWhenRead">
        /// The actions when read.
        /// </param>
        private async Task ReadDataFromDbDataReader(IEnumerable<Action<DbDataReader>> actionsWhenRead)
        {
            using (DbDataReader reader = await this.Command.ExecuteReaderAsync())
            {
                foreach (var action in actionsWhenRead)
                {
                    while (reader.Read())
                    {
                        action(reader);
                    }

                    reader.NextResult();
                }
            }
        }

        /// <summary>
        /// The set connection and transaction.
        /// </summary>
        private void SetConnectionAndTransaction()
        {
            this.Command.Connection = this.Connection;

            if (this.transaction != null)
            {
                this.Command.Transaction = this.transaction;
            }
        }

        /// <summary>
        /// The set value to db null if null.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        private void SetValueToDbNullIfNull(ref object value)
        {
            if (value == null)
            {
                value = DBNull.Value;
            }
        }

        #endregion
    }
}