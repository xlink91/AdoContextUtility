using AdoContextUtility.Attributes.MappingAttribute;
using AdoContextUtility.Common;
using AdoContextUtility.Contract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdoContextUtility.Implementation
{
    public class Map : IMap
    {
        private static IMap _instance { get; set; }
        private static readonly object objLock = new object();

        public static IMap Instance
        {
            get
            {
                if(_instance == null)
                {
                    lock (objLock)
                    {
                        _instance = _instance ?? new Map();
                    }
                }
                return _instance;
            }
        }

        public List<TTo> MapData<TFrom, TTo>((DynamicResult, IList) result)
            where TTo : new()
        {
            List<TTo> resultList = new List<TTo>();
            Type typeEntity = typeof(TTo);
            List<string> dynamicMemberNames = result.Item1.GetDynamicMemberNames().ToList();
            IDictionary<string, string> fieldMapper = MakePropertiesMapper<TTo>(dynamicMemberNames);
            foreach (var nEntity in result.Item2)
            {
                TTo ent = new TTo();
                foreach (var propName in dynamicMemberNames)
                {
                    if (fieldMapper.ContainsKey(propName))
                        typeEntity.GetProperty(fieldMapper[propName]).SetValue(ent, ((DynamicResult)nEntity)[propName]);
                }
                resultList.Add(ent);
            }
            return resultList;
        }

        private IDictionary<string, string> MakePropertiesMapper<TEntity>(List<string> dynamicProperties)
        {
            IDictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (var prop in typeof(TEntity).GetProperties())
            {
                MapperAttribute mapperAttribute = prop.GetCustomAttribute<MapperAttribute>();

                if (mapperAttribute == null)
                {
                    if (dynamicProperties.Contains(prop.Name))
                        keyValuePairs[prop.Name] = prop.Name;
                    continue;
                }

                if (dynamicProperties.Contains(mapperAttribute.FieldName))
                    keyValuePairs[mapperAttribute.FieldName] = prop.Name;
            }
            return keyValuePairs;
        }
    }
}
