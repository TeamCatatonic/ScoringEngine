using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoringEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine($"Usage: {Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location)} in_file_path out_file_path");
                return;
            }

            if (!File.Exists(args[1]))
                File.Create(args[1]).Close();

            System.Diagnostics.Process.Start(args[1]);

            List<ScoredItem> items = ScoredItemParser.ParseFromFile(args[0]);
            while (true)
            {
                ScoredItemParser.ScoreToFile(args[1], items);
                System.Threading.Thread.Sleep(10000);
            }
        }
    }
}
