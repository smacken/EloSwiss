using System.Globalization;
using FluentAssertions;
using Xunit;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Linq;
using EloSwissCli;

namespace EloSwiss.Tests
{
    public class MatchParserTests : IClassFixture<CsvFixture>
    {
        CsvFixture fixture;
        public MatchParserTests(CsvFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void CanReadCsv()
        {
            var reader = new StreamReader("matches.csv");
            reader.Peek().Should().NotBe(-1);
            var csv = new CsvReader(reader, CultureInfo.CurrentCulture);
            csv.Should().NotBeNull();
            
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            { 
                PrepareHeaderForMatch = args => args.Header.ToLowerInvariant()
            };
            csv.GetRecords<EloSwissMatch>().Should().NotBeEmpty();
        }

        [Fact]
        public void CanReadCsv_CanTransform()
        {
            var reader = new StreamReader("matches.csv");
            reader.Peek().Should().NotBe(-1);
            var csv = new CsvReader(reader, CultureInfo.CurrentCulture);
            csv.Should().NotBeNull();
            
            var matches = csv.GetRecords<EloSwissMatch>();
            var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            { 
                PrepareHeaderForMatch = args => args.Header.ToLowerInvariant()
            };
            var match = matches.First();
            var elo = match.AsMatch();
            elo.Player1.Should().Be("A");
            elo.Player2.Should().Be("B");
            elo.Winner.Should().Be(Winner.Player2);
        }
    }
}