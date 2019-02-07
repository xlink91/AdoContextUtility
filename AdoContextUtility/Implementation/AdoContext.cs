using AdoContextUtility.Common;
using AdoContextUtility.Contract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AdoContextUtility.Implementation
{
    public class AdoContext : IAdoContext
    {
        protected string ConnectionString { get; set; }
        public AdoContext(string ConnectionString = null)
        {
            this.ConnectionString = ConnectionString;
        }
         
        public (DynamicResult, IList) Execute(StoreProcedureInfo storeProcedureInfo, string ConnectionString = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString ?? this.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(storeProcedureInfo.StoreProcedureName, sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    foreach (KeyValuePair<string, SqlParameter> param in storeProcedureInfo.Parameters)
                        sqlCommand.Parameters.Add(param.Value);

                    try
                    {
                        sqlConnection.Open();
                        SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                        DataTable schemaTable = sqlDataReader.GetSchemaTable();
                        Dictionary<string, object> mapper = new Dictionary<string, object>();

                        foreach (var parameterInfo in storeProcedureInfo.Parameters)
                        {
                            if (sqlCommand.Parameters[parameterInfo.Key].Direction > ParameterDirection.Input)
                            {
                                var vl = sqlCommand.Parameters[parameterInfo.Key].Value;
                                mapper[parameterInfo.Key] = vl == DBNull.Value ? null : vl;
                            }
                        }

                        IList ls = new ArrayList();
                        object value = null;
                        for (; sqlDataReader.Read();)
                        {
                            Dictionary<string, object> _entity = new Dictionary<string, object>();
                            for (int i = 0; i < sqlDataReader.FieldCount; ++i)
                            {
                                if (sqlDataReader.GetValue(i) is DBNull)
                                    value = null;
                                else
                                    value = Convert.ChangeType(sqlDataReader.GetValue(i), sqlDataReader.GetFieldType(i));
                                _entity[sqlDataReader.GetName(i)] = value;
                            }
                            ls.Add(new DynamicResult(_entity));
                        }


                        return (new DynamicResult(mapper), ls);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}
