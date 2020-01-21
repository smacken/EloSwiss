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

        [Fact]
        public void Ratings_ScoresMatchWithWinner_Dominant()
        {
            var results = Elo.Score(1200, 1000, Winner.Player1);
            results.ratingA.Should().BeGreaterThan(results.ratingB);
        }

        [Fact]
        public void Ratings_ScoresMatchWithWinner_Underdog()
        {
            var results = Elo.Score(1200, 1000, Winner.Player2);
            results.ratingA.Should().BeGreaterThan(results.ratingB);
        }

        [Fact]
        public void Ratings_ScoresMatch_WithHomeAdvantage()
        {
            var resultsHome = Elo.Score(1000, 1000, Winner.Player1, Played.Home);
            var results = Elo.Score(1000, 1000, Winner.Player1);
            results.ratingA.Should().BeGreaterThan(results.ratingB);
            resultsHome.ratingA.Should().BeGreaterThan(results.ratingA);
        }

        [Fact]
        public void Ratings_ScoresMatch_WithHomeAdvantage_RevisedHFA()
        {
            var resultsHome = Elo.Score(1500, 1500, Winner.Player1, Played.Home, kFactor: 20);
            var results = Elo.Score(1500, 1500, Winner.Player1, 20);
            results.ratingA.Should().BeGreaterThan(results.ratingB);
            resultsHome.ratingA.Should().BeGreaterThan(results.ratingA);
        }
    }
}
