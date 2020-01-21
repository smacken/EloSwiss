using CsvHelper;
using System.IO;
using System.Collections.Generic;
using EloSwissCli;
using System;
using System.Globalization;

namespace EloSwiss.Tests
{
    public class CsvFixture : IDisposable
    {
        public CsvFixture()
        {
            using var reader = new StreamReader("matches.csv");
            using var csv = new CsvReader(reader, CultureInfo.CurrentCulture);
            csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
            // csv.Configuration.HasHeaderRecord = false;
            Matches = csv.GetRecords<EloSwissMatch>();
        }

        public void Dispose()
        {
            Matches = null;
        }

        public IEnumerable<EloSwissMatch> Matches { get; private set; }
    }
}