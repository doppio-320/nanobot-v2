using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2
{
    public class RNGManager
    {
        private static Random random;

        public static void Create()
        {
            random = new Random();
            ProgramLogger.LogImportant("RNGManager initialized [TRACEBACK SEED: -1]");
        }

        public static uint RandomID()
        {
            uint thirtyBits = (uint)random.Next(1 << 30);
            uint twoBits = (uint)random.Next(1 << 2);
            uint fullRange = (thirtyBits << 2) | twoBits;

            return fullRange;
        }
    }
}
