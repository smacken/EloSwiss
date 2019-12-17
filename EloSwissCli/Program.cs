using System;
using System.IO;
using System.Linq;
using CommandLine;
using CsvHelper;
using EloSwiss;

namespace EloSwissCli
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("EloSwiss");
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(option =>
                {
                    if (option.Verbose)
                    {
                        Console.WriteLine($"Verbose output enabled. Current Arguments: -v {option.Verbose}");
                        Console.WriteLine("EloSwiss Tournament: Verbose");
                    }
                    else
                    {
                        Console.WriteLine($"Current Arguments: -v {option.Verbose}");
                        Console.WriteLine("EloSwiss Tournament");
                    }

                    bool hasExisting = !string.IsNullOrEmpty(option.Csv);
                    if (hasExisting)
                    {
                        using var reader = new StreamReader(option.Csv);
                        using var csv = new CsvReader(reader);
                        csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                        var matches = csv.GetRecords<EloMatch>();
                    }
                    
                    bool hasTournamentSetup = !string.IsNullOrEmpty(option.Setup);
                    if (hasTournamentSetup && File.Exists(option.Setup))
                    {
                        Tournament tournament = option.Setup.FromJson();
                        tournament.Rounds = new Swiss()
                            .BuildRounds(tournament, Enumerable.Empty<Round>(), tournament.RoundCount)
                            .ToList();
                        tournament.ToJson(option.OutputFile);
                    }
                });
        }
    }
}
