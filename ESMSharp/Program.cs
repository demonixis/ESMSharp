using ESMSharp.Core;
using ESMSharp.TES4;
using System;
using System.Diagnostics;

namespace ESMSharp
{
    class Program
    {
        private static void Main(string[] args)
        {
            Utils.LogToFile = true;

            //var tes3 = new TES3Master("Data/morrowind.esm");

            var filename = "Fallout4.esm";
            var watch = new Stopwatch();

            WriteStart(watch, $"Parsing {filename}");
            var tes4 = new TES4Master(filename);
            WriteStop(watch);

            //WriteStart(watch, "Try to load archives...");
            //var fs = new FileSystem("Data/.");
            //WriteStop(watch);

            Console.WriteLine("END");
            Console.ReadKey();
        }

        private static void WriteStart(Stopwatch watch, string prefix)
        {
            Console.WriteLine(prefix);
            watch.Restart();
        }

        private static void WriteStop(Stopwatch watch, string prefix = "")
        {
            watch.Stop();
            var time = TimeSpan.FromTicks(watch.ElapsedTicks);
            Console.WriteLine($"{prefix}Loaded in {time.TotalSeconds} seconds");
        }
    }
}
