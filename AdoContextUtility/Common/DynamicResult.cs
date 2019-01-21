using System.Collections.Generic;
using System.Dynamic;

namespace AdoContextUtility.Common
{
    public class DynamicResult : DynamicObject
    {
        private IDictionary<string, object> keyValuePairs = new Dictionary<string, object>();

        public DynamicResult(IDictionary<string, object> keyValuePairs)
        {
            this.keyValuePairs = keyValuePairs;
        }

        public object this[string name]
        {
            get
            {
                return keyValuePairs[name];
            }
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return keyValuePairs.Keys;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!keyValuePairs.ContainsKey(binder.Name))
                return false;
            keyValuePairs.Add(binder.Name, value);
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return keyValuePairs.TryGetValue(binder.Name, out result);
        }
    }
}
