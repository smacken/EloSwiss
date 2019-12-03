using System;
using FluentAssertions;
using Xunit;

namespace EloSwiss.Tests
{
    public class EloTests
    {
        [Fact]
        public void Ratings_ScoresMatch()
        {
            var results = Elo.Score(1000, 1000, 1, 0);
            results.ratingA.Should().BeGreaterThan(results.ratingB);
        }

        [Fact]
        public void Ratings_ScoresMatchWithWinner()
        {
            var results = Elo.Score(1000, 1000, Winner.Player1);
            results.ratingA.Should().BeGreaterThan(results.ratingB);
        }
    }
}
