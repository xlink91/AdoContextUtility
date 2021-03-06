﻿using AdoContextUtility.Attributes.AdoAttribute;
using AdoContextUtility.Common;
using AdoContextUtility.Contract;
using AdoContextUtility.Implementation;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AdoContextUtility.Extension
{
    public static class AdoContextExtension
    {
        public static (dynamic, IList ls) Execute<TEntity>(this AdoContext adoContext, TEntity entity)
        {
            Type type = typeof(TEntity);
            Type spi = typeof(StoreProcedureInfo);
            StoreProcedureInfo sp = new StoreProcedureInfo(type.Name);

            foreach (PropertyInfo prop in type.GetProperties())
            {
                AdoEntityDescriptionAttribute attribute = prop.GetCustomAttribute<AdoEntityDescriptionAttribute>();
                if (attribute == null)
                    continue;

                SqlParameter sqlp = new SqlParameter(prop.Name, prop.GetValue(entity));
                sqlp.SqlDbType = attribute.dbType != (SqlDbType)int.MaxValue ? attribute.dbType : sqlp.SqlDbType;
                sqlp.Size = attribute.size != -1 ? attribute.size : sqlp.Size;
                sqlp.Direction = attribute.direction != 0 ? attribute.direction : sqlp.Direction;
                sqlp.Precision = attribute.precision != 0 ? attribute.precision : sqlp.Precision;
                sqlp.Scale = attribute.scale != 0 ? attribute.scale : sqlp.Scale;
                sqlp.SourceColumn = attribute.sourceColumn ?? sqlp.SourceColumn;
                sqlp.SourceVersion = attribute.sourceVersion != 0 ? attribute.sourceVersion : sqlp.SourceVersion;
                sp[prop.Name] = sqlp;
            }
            return adoContext.Execute(sp);
        }
    }
}
