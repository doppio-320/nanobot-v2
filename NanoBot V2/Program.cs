using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBot_V2
{
    class Program
    {
        static void Main(string[] args)
        {
            ProgramLogger.Load();
            new BotRuntime().Run();
            ProgramLogger.Unload();
        }
    }
}
