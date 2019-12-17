using System;
using System.IO;
using CommandLine;
using CsvHelper;

namespace EloSwissCli
{
    class Program
    {
        public class Options
        {
            [Value(0, MetaName = "csv", HelpText = "CSV File with matches.")]
            public string Csv { get; set; }

            [Option("output",
                Default = false,
                HelpText = "Output to file")]
            public bool output { get; set; }

            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("EloSwiss");
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(option =>
                {
                    using var reader = new StreamReader(option.Csv);
                    using var csv = new CsvReader(reader);
                    csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                    var matches = csv.GetRecords<EloMatch>();

                    if (option.Verbose)
                    {
                        Console.WriteLine($"Verbose output enabled. Current Arguments: -v {option.Verbose}");
                        Console.WriteLine("Quick Start Example! App is in Verbose mode!");
                    }
                    else
                    {
                        Console.WriteLine($"Current Arguments: -v {option.Verbose}");
                        Console.WriteLine("Quick Start Example!");
                    }
                });
        }
    }
}
