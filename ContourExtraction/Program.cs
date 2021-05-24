using System;
using System.IO;
using System.Reflection;

namespace ContourExtraction
{
    class Program
    {
        public static string filepath = "";
        public static string keepInstancesAlive = "";

        static void Main(string[] args)
        {
            var yuv = new YuvModel();
            var helpers = new Helpers(yuv);
            var actions = new Actions(Console.ReadLine, Console.Write, yuv, helpers);
            var controller = new CustomController(Console.ReadLine, Console.Write, actions);

            
            ConfigStartup(args);

            
            while (true)
            {

                Console.Clear();

                GetAssemblyInfo();

                controller.Build()
                          .Run()
                          .Out();

            }

        }


        private static void ConfigStartup(string[] args)
        {

            if (args.Length is 0)
            {
                Help();
                Environment.Exit(0);
            }
            else if (args[0] is "-h" || args[0] is "--help")
            {
                Help();
                Environment.Exit(0);
            }
            else if (args.Length is 2 && args[0] is "run")
            {
                var input = args[1];
                var file = Path.GetFileName(input) ?? string.Empty;

                if (file is not "" && file.EndsWith(".yuv"))
                {
                    filepath = input;
                }
                else if (file is "")
                {
                    Console.WriteLine($"'{input}' is not a file.\n");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine($"'{file}' is not recognized as a known file type.\n");
                    Console.WriteLine("A file extension of type '.yuv' is expected.\n");
                    Environment.Exit(0);
                }
            }
            else
            {
                Help();
                Environment.Exit(0);
            }
        }


        private static void GetAssemblyInfo()
        {
            Console.Clear();

            Console.Write($"{Assembly.GetAssembly(typeof(Program)).GetName().Name} ");
            Console.Write($"[Version {Assembly.GetAssembly(typeof(Program)).GetName().Version}]");
            Console.WriteLine("\n(c) 2021 Alex Varypatis. All rights reserved.\n");
        }


        private static void Help()
        {
            Console.WriteLine($"\nUsage: executable run [path-to-file]");
            Console.WriteLine("\npath-to-file:");
            Console.WriteLine("  The path to a .yuv file to run.");
        }

    }
}
