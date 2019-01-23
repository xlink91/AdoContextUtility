using System;

namespace AdoContextUtility.Attributes.MappingAttribute
{
    public class MapperAttribute : Attribute
    {
        public string FieldName { get; set; }
    }
}
