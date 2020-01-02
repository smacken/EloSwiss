using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace EloSwiss.Tests
{
    public class StandingTests
    {
        [Fact]
        public void Standing_GetsPlayerRanking()
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
            var rounds = swiss.BuildRounds(tournament, Enumerable.Empty<Round>(), tournament.RoundCount);
            rounds.Should().NotBeEmpty();
            tournament.Should().NotBeNull();
            foreach (var round in rounds)
            {
                foreach (var match in round.Matches)
                {
                    match.Winner = Winner.Player1;
                    match.Score();
                }
            }
            var standings = tournament.CurrentStandings();
            standings.Should().NotBeEmpty();
        }
    }
}