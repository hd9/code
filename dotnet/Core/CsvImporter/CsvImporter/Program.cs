using System;

namespace CsvImporter
{
    class Program
    {

        const string bar = "------------------------------------------";

        static void Main(string[] args)
        {
            Header();

            // tries to load filename
            if (args.Length == 2)
            {
                new CsvImporterDemo().Import(args[1]);
            }
            else
            {
                Console.WriteLine("No valid file provided...");
            }


            Footer();
        }

        private static void Header()
        {
            Console.WriteLine(bar);
            Console.WriteLine("[Demo] A simple CSV Reader in .Net Core");
            Console.WriteLine(bar);
        }

        private static void Footer()
        {
            Console.WriteLine("\r\n" + bar);
            Console.WriteLine("[Demo] Bye!");
            Console.WriteLine(bar);
        }
    }
}
