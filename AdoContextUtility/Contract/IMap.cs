using AdoContextUtility.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoContextUtility.Contract
{
    public interface IMap
    {
        List<TTo> MapData<TFrom, TTo>((DynamicResult, IList) result)
            where TTo : new();
    }
}
