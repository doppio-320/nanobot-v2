using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2.Services
{
    public static class UlongConv
    {
        public static long UlongToLong(this ulong _orig)
        {
            return Convert.ToInt64(_orig);
        }

        public static ulong LongToUlong(this long _orig)
        {
            return Convert.ToUInt64(_orig);
        }
    }
}
