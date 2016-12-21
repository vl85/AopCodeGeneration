using System;
using System.Diagnostics;

namespace AopCodeGeneration
{
        class Program
        {
            static void Main(string[] args)
            {
                if (args == null) throw new ArgumentNullException(nameof(args));


                Debugger.Break();
            }
        }
}
