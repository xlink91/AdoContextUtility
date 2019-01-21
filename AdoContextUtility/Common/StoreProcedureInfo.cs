using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace AdoContextUtility.Common
{
    public class StoreProcedureInfo
    {
        public string StoreProcedureName { get; private set; }
        public Dictionary<string, SqlParameter> Parameters { get; set; }
        private SqlParameter lastAdded { get; set; }

        public StoreProcedureInfo(string storeProcedureName)
        {
            this.StoreProcedureName = storeProcedureName;
            Parameters = new Dictionary<string, SqlParameter>();
        }

        public SqlParameter this[string name]
        {
            get
            {
                if (!Parameters.ContainsKey(name))
                    throw new Exception($"Key {name} doesn't exists");
                return Parameters[name];
            }
            set
            {
                Parameters[name] = value;
            }
        }

        public StoreProcedureInfo AddParameter<TType>(Expression<Func<TType>> exp)
        {
            (string name, object value) = GetValue(exp);
            SqlParameter sqlParameter = new SqlParameter(name, value);
            Parameters.Add(name, sqlParameter);
            lastAdded = sqlParameter;
            return this;
        }

        public StoreProcedureInfo Setting(SqlDbType? dbType = null,
            int? size = null,
            ParameterDirection? direction = null,
            byte? precision = null,
            byte? scale = null,
            string sourceColumn = null,
            DataRowVersion? sourceVersion = null,
            bool? sourceColumnNullMapping = null,
            string xmlSchemaCollectionDatabase = null,
            string xmlSchemaCollectionOwningSchema = null,
            string xmlSchemaCollectionName = null)
        {
            if (lastAdded == null)
                throw new Exception("There's not element inserted");
            lastAdded.Size = size ?? lastAdded.Size;
            lastAdded.Direction = direction ?? lastAdded.Direction;
            lastAdded.Precision = precision ?? lastAdded.Precision;
            lastAdded.Scale = scale ?? lastAdded.Scale;
            lastAdded.SourceColumn = sourceColumn ?? lastAdded.SourceColumn;
            lastAdded.SourceVersion = sourceVersion ?? lastAdded.SourceVersion;
            lastAdded.SourceColumnNullMapping = sourceColumnNullMapping ?? lastAdded.SourceColumnNullMapping;
            lastAdded.XmlSchemaCollectionDatabase = xmlSchemaCollectionDatabase ?? lastAdded.XmlSchemaCollectionDatabase;
            lastAdded.XmlSchemaCollectionOwningSchema = xmlSchemaCollectionOwningSchema ?? lastAdded.XmlSchemaCollectionOwningSchema;
            lastAdded.XmlSchemaCollectionName = xmlSchemaCollectionName ?? lastAdded.XmlSchemaCollectionName;
            return this;
        }

        private (string name, object value) GetValue<TType>(Expression<Func<TType>> exp)
        {
            Expression expression = (MemberExpression)exp.Body;
            MemberExpression mExp = (MemberExpression)expression;

            return (mExp.Member.Name, Convert.ChangeType(Expression.Lambda(mExp).Compile().DynamicInvoke(), mExp.Type));
        }
    }
}
