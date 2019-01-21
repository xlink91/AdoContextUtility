using System;
using System.Data;

namespace AdoContextUtility.AdoAttribute
{
    public class AdoEntityDescriptionAttribute : Attribute
    {
        public SqlDbType dbType = (SqlDbType)int.MaxValue;
        public int size = -1;
        public ParameterDirection direction = 0;
        public byte precision = 0;
        public byte scale = 0;
        public string sourceColumn = null;
        public DataRowVersion sourceVersion = 0;
    }
}
