using FluentAssertions;
using Xunit;

namespace EloSwiss.Tests
{
    public class MatchTests
    {
        [Fact]
        public void Match_BetweenTwoEqual()
        {
            var p1 = new Player("1",1000);
            var p2 = new Player("2", 1000);
            var match = new Match {Player1 = p1, Player2 = p2, Winner = Winner.Player1};
            match.Score();
            p1.Rating.Should().BeGreaterThan(1000);
            p2.Rating.Should().BeLessThan(1000);
        }

        [Fact]
        public void Match_UnderdogWins()
        {
            var p1 = new Player("1", 1000);
            var p2 = new Player("2", 1500);
            var match = new Match { Player1 = p1, Player2 = p2, Winner = Winner.Player1 };
            match.Score();
            p1.Rating.Should().BeGreaterThan(1000);
            p2.Rating.Should().BeLessThan(1500);
        }

        [Fact]
        public void Match_FavouriteWins()
        {
            var p1 = new Player("1", 1500);
            var p2 = new Player("2", 900);
            var match = new Match { Player1 = p1, Player2 = p2, Winner = Winner.Player1 };
            match.Score();
            p1.Rating.Should().BeGreaterThan(1500);
            p2.Rating.Should().BeLessThan(900);
        }

        [Fact]
        public void Match_Bye()
        {
            var p1 = new Player("1", 1500);
            var match = new ByeMatch(p1);
            match.Score();
            p1.Rating.Should().BeGreaterThan(1500);
        }
    }
}