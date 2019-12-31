using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace EloSwiss.Tests
{
    public class SwissTests
    {
        [Fact]
        public void Tournament_BuildsMatches()
        {
            var players = new List<Player>
            {
                new Player("A"), 
                new Player("B"),
                new Player("C"),
                new Player("D")
            };
            var tournament = new Tournament{Players = players};
            var swiss = new Swiss();
            var matches = swiss.BuildMatchPairs(tournament.Players, tournament);
            matches.Should().NotBeEmpty();
            tournament.Should().NotBeNull();
        }

        [Fact]
        public void Tournament_BuildsRounds()
        {
            var players = new List<Player>
            {
                new Player("A"),
                new Player("B"),
                new Player("C"),
                new Player("D")
            };
            var tournament = new Tournament { Players = players };
            var swiss = new Swiss();
            var rounds = swiss.BuildRounds(tournament, Enumerable.Empty<Round>(), tournament.RoundCount);
            rounds.Should().NotBeEmpty();
        }

        [Fact]
        public void Tournament_BuildsRoundsSeeded()
        {
            var players = new List<Player>
            {
                new Player("A") {Seed=1},
                new Player("B") {Seed=2},
                new Player("C") {Seed=3},
                new Player("D") {Seed=4}
            };
            var tournament = new Tournament { Players = players };
            var swiss = new Swiss();
            var rounds = swiss.BuildRounds(tournament, Enumerable.Empty<Round>(), tournament.RoundCount);
            rounds.Should().NotBeEmpty();
        }

        [Fact]
        public void Tournament_BuildsRound_WithBye()
        {
            var players = new List<Player>
            {
                new Player("A"),
                new Player("B"),
                new Player("C"),
                new Player("D"),
                new Player("E")
            };
            var tournament = new Tournament { Players = players };
            var swiss = new Swiss();
            var rounds = swiss.BuildRounds(tournament, Enumerable.Empty<Round>(), tournament.RoundCount);
            rounds.Should().NotBeEmpty();
        }

        [Fact]
        public void Tournament_MaxRounds()
        {
            var players = new List<Player>
            {
                new Player("A"),
                new Player("B"),
                new Player("C"),
                new Player("D")
            };
            var tournament = new Tournament { Players = players };
            tournament.RoundCount.Should().Be(2);
        }

        [Fact]
        public void Tournament_MaxRounds_WithOddCount()
        {
            var players = new List<Player>
            {
                new Player("A"),
                new Player("B"),
                new Player("C"),
                new Player("D"),
                new Player("E")
            };
            var tournament = new Tournament { Players = players };
            tournament.RoundCount.Should().Be(3);
            tournament.Rounds = new Swiss().BuildRounds(tournament, Enumerable.Empty<Round>(), tournament.RoundCount).ToList();
            tournament.Rounds.Should().NotBeEmpty();
        }

        private List<Player> playoffPlayers = new List<Player>
            {
                new Player("A", 1020),
                new Player("B", 1019),
                new Player("C", 1018),
                new Player("D", 1017),
                new Player("E", 1016),
                new Player("F", 1015),
                new Player("G", 1014),
                new Player("H", 1013),
                new Player("I", 1012),
                new Player("J", 1011),
                new Player("K", 1010),
                new Player("L", 1009),
                new Player("M", 1008),
                new Player("N", 1007),
                new Player("O", 1006),
                new Player("P", 1005),
                new Player("Q", 1004),
                new Player("R", 1003),
            };

        [Fact]
        public void Tournament_BuildsPlayoffs()
        {
            var tournament = new Tournament { Players = playoffPlayers };
            var swiss = new Swiss();
            var rounds = swiss.BuildPlayoffRound(tournament, PlayoffRound.Playoff);
            rounds.Should().NotBeEmpty();
            rounds.SelectMany(x => x.Players).Count().Should().Be(16);

            var quarters = swiss.BuildPlayoffRound(tournament, PlayoffRound.QuarterFinal);
            quarters.Should().NotBeEmpty();
            quarters.SelectMany(x => x.Players).Count().Should().Be(8);

            var semis = swiss.BuildPlayoffRound(tournament, PlayoffRound.SemiFinal);
            semis.Should().NotBeEmpty();
            semis.SelectMany(x => x.Players).Count().Should().Be(4);

            var final = swiss.BuildPlayoffRound(tournament, PlayoffRound.Final);
            final.Should().NotBeEmpty();
            final.SelectMany(x => x.Players).Count().Should().Be(2);
        }

        [Fact]
        public void Tournament_BuildsPlayoffs_WithCutoff()
        {
            var tournament = new Tournament { Players = playoffPlayers.Take(7).ToList() };
            var swiss = new Swiss();
            var rounds = swiss.BuildPlayoffRound(tournament, PlayoffRound.Playoff);
            rounds.Should().NotBeEmpty();
            rounds.SelectMany(x => x.Players).Count().Should().Be(4);
        }
    }
}