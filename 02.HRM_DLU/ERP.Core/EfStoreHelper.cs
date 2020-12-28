using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;

namespace ERP_Core
{
    public static class EfStoreHelper
    {
        #region CONST
        private const string ConstProviderName = "System.Data.SqlClient";
        #endregion

        public static DbCommand CreateCommand(DbConnection storeConnection,
              string cmdText, CommandType cmdType, List<DbParameter> parameters)
        {
            DbCommand command = storeConnection.CreateCommand();
            command.CommandText = cmdText;
            command.CommandType = cmdType;
            if (parameters != null)
            {
                foreach (DbParameter p in parameters)
                {
                    command.Parameters.Add(p);
                }
            }
            return command;
        }


        public static List<T> FillEntities<T>(ObjectContext context,
                     string cmdText, CommandType cmdType,
                     List<DbParameter> parameters,
                     ConnectionState connectionState)
        {
            DbConnection connection = context.Connection;

            DbDataReader reader = null;
            try
            {
                DbCommand cmd = CreateCommand(connection, cmdText, cmdType, parameters);
                reader = cmd.ExecuteReader();
                ObjectResult<T> result = context.Translate<T>(reader);
                return result.ToList<T>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connectionState == ConnectionState.Closed)
                {
                    connection.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
        }
    }
}
