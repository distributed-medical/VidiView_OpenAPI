using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidiView.Api;

internal static class GuidComparer
{
    public static bool IsEitherNullOrEqual(Guid? a, Guid? b)
    {
        if (a == null || b == null)
            return true;
        return a.Value.Equals(b.Value);
    }
}
