using AdoContextUtility.Common;
using System.Collections;

namespace AdoContextUtility.Contract
{
    public interface IAdoContext
    {
        (DynamicResult, IList) Execute(StoreProcedureInfo storeProcedureInfo, string ConnectionString = null);
    }
}
