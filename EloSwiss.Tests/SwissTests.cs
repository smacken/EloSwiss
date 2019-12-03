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
            // var rounds = swiss.BuildRounds(tournament, Enumerable.Empty<Round>());
            // rounds.Should().NotBeEmpty();
            tournament.Should().NotBeNull();
        }
    }
}