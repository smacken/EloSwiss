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
            var matches = swiss.BuildMatchPairs(tournament.Players);
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
    }
}