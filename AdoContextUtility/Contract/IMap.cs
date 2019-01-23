using AdoContextUtility.Common;
using System.Collections;
using System.Collections.Generic;

namespace AdoContextUtility.Contract
{
    public interface IMap
    {
        List<TTo> MapData<TFrom, TTo>((DynamicResult, IList) result)
            where TTo : new();
    }
}
