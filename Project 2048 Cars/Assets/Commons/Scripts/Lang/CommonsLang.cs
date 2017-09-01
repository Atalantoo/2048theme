using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Commons.Lang
{
    public class CommonsLang
    {
    }

    // https://msdn.microsoft.com/fr-fr/library/system.diagnostics.contracts.contract.requires(v=vs.110).aspx
    public class Contract
    {
        internal static void Requires<T>(bool condition)
        {
            if (!condition)
                throw new NullReferenceException();
        }
    }
}
