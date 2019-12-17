using CommandLine;

namespace EloSwissCli
{
    partial class Program
    {
        public class Options
        {
            [Value(0, MetaName = "csv", HelpText = "CSV File with matches.")]
            public string Csv { get; set; }

            [Value(1, MetaName = "setup", HelpText = "Tournament setup file including players.")]
            public string Setup { get; set; }

            [Option("output",
                Default = false,
                HelpText = "Output to file")]
            public bool Output { get; set; }

            [Value(1, MetaName = "outputFile", HelpText = "Tournament output file.")]
            public string OutputFile{ get; set; }

            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }
        }
    }
}
