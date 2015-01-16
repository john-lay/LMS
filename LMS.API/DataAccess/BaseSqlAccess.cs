namespace LMS.API.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;

    public abstract class BaseSqlAccess
    {
        CommandType commandType = CommandType.Text;

        protected DbTransaction transaction = null;
        protected DbConnection Connection { get; private set; }

        protected DbCommand Command { get; private set; }

        string connectionString;

        #region Protected abstract methods
        #region GetDbConnectionCommand
        protected abstract DbConnection GetDbConnection();
        protected abstract DbCommand GetDbCommand();
        #endregion

        #region GetDbParameter
        protected abstract DbParameter GetDbParameter(string name, object value);

        protected abstract DbParameter GetDbParameter(string name, DbType type, object value);

        protected abstract DbParameter GetDbParameter(string name, DbType type);

        protected abstract DbParameter GetDbParameter(string name, DbType type, ParameterDirection direction);
        #endregion

        protected abstract string GetDefaultConnectionStrig();
        #endregion

        #region Constructors
        public BaseSqlAccess(string connectionString, string sqlCommand, CommandType commandType)
        {
            this.commandType = commandType;

            this.connectionString = connectionString;
            Command = GetDbCommand();
            Command.CommandText = sqlCommand;
            Command.CommandType = commandType;
        }

        public BaseSqlAccess(string sqlCommand, CommandType commandType)
            : this(null, sqlCommand, commandType)
        {
        }

        public BaseSqlAccess(string connectionString, string storedProcedureName)
            : this(connectionString, storedProcedureName, CommandType.StoredProcedure)
        {
        }

        public BaseSqlAccess(string storedProcedureName)
            : this(null, storedProcedureName, CommandType.StoredProcedure)
        {
        }
        #endregion

        /// <summary>
        /// Add a parameter to parameters collection of command. If value is null or is String.Empty sets the value to DBNull.Value
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="value">Value of parameter</param>
        public void AddParameter(string name, object value)
        {
            SetValueToDbNullIfNull(ref value);

            Command.Parameters.Add(GetDbParameter(name, value));
        }

        /// <summary>
        /// Add a parameter to parameters collection of command. If value is null or is String.Empty sets the value to DBNull.Value
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="type">DbType of parameter</param>
        /// <param name="value">Value of parameter</param>
        public void AddParameter(string name, DbType type, object value)
        {
            SetValueToDbNullIfNull(ref value);
            Command.Parameters.Add(GetDbParameter(name, type, value));
        }

        public void AddStrParamAndSetToDbNullIfNullOrEmpty(string name, string value)
        {
            AddParameter(name, String.IsNullOrEmpty(value) ? null : value);
        }

        public DbParameter AddParameter(DbParameter parameter)
        {
            Command.Parameters.Add(parameter);
            return parameter;
        }

        protected DbConnection GetOpenConnectionAfterAssignToCommand()
        {
            this.OpenConnection(false);

            Command.Connection = Connection;
            return Connection;
        }

        public void OpenConnection(bool openTransaction)
        {
            Connection = GetDbConnection();
            if (String.IsNullOrEmpty(connectionString))
                connectionString = DefaultConnectionString;

            Connection.ConnectionString = connectionString;
            Connection.Open();

            if (openTransaction)
                this.transaction = Connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (this.transaction != null)
                this.transaction.Commit();
        }

        public void RollbackTransaction()
        {
            if (this.transaction != null)
                this.transaction.Rollback();
        }

        public void CloseConnection()
        {
            if (Connection != null && Connection.State != ConnectionState.Closed)
                Connection.Close();
        }

        void SetValueToDbNullIfNull(ref object value)
        {
            if (value == null)
                value = DBNull.Value;
        }

        public int ExecuteNonQuery()
        {
            using (GetOpenConnectionAfterAssignToCommand())
                return Command.ExecuteNonQuery();
        }

        public int ExecuteNonQueryInOpenedTransaction()
        {
            SetConnectionAndTransaction();

            return Command.ExecuteNonQuery();
        }

        public void ResetCommand(string commandText)
        {
            Command = this.GetDbCommand();
            Command.CommandText = commandText;
            Command.CommandType = this.commandType;
        }

        public object ExecuteScalar()
        {
            using (GetOpenConnectionAfterAssignToCommand())
                return Command.ExecuteScalar();
        }

        public void ExecuteReader(params Action<DbDataReader>[] actionsWhenRead)
        {
            using (GetOpenConnectionAfterAssignToCommand())
            {
                ReadDataFromDbDataReader(actionsWhenRead);
            }
        }

        public void ExecuteReaderInOpenedTransaction(params Action<DbDataReader>[] actionsWhenRead)
        {
            SetConnectionAndTransaction();

            ReadDataFromDbDataReader(actionsWhenRead);
        }

        public T ExecuteReaderFirstRowIfExists<T>(Func<DbDataReader, T> functionWhenRead)
        {
            using (GetOpenConnectionAfterAssignToCommand())
            {
                using (var reader = Command.ExecuteReader())
                {
                    if (reader.Read())
                        return functionWhenRead(reader);
                    return default(T);
                }
            }
        }

        public List<T> ToListByConvertFunctionFromReader<T>(Func<DbDataReader, T> fromReaderToTypeT)
        {
            var listOfEntities = new List<T>();
            ExecuteReader(reader =>
            {
                var entity = fromReaderToTypeT(reader);
                listOfEntities.Add(entity);
            });
            return listOfEntities;
        }

        /// <summary>
        /// Executes actions that use SQL access in the same transaction. You need to call ResetCommand and Excute...InOpenedTransaction inside the actions
        /// </summary>
        /// <param name="actions">Actions to execute inside the same SQL transaction</param>
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

        public void SetCommandTimeout(int timeout)
        {
            Command.CommandTimeout = timeout;
        }

        public string DefaultConnectionString
        {
            get { return GetDefaultConnectionStrig(); }
        }

        protected string GetConnectionStringByName(string name)
        {
            var connString = ConfigurationManager.ConnectionStrings[name];
            if (connString == null)
                return null;
            return connString.ConnectionString;
        }

        void SetConnectionAndTransaction()
        {
            Command.Connection = Connection;

            if (this.transaction != null)
                Command.Transaction = this.transaction;
        }

        void ReadDataFromDbDataReader(IEnumerable<Action<DbDataReader>> actionsWhenRead)
        {
            using (var reader = Command.ExecuteReader())
            {
                foreach (var action in actionsWhenRead)
                {
                    while (reader.Read())
                        action(reader);
                    reader.NextResult();
                }
            }
        }
    }
}
