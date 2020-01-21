using System;

namespace EloSwiss
{
    public enum Winner
    {
        Player1, Player2
    }

    public enum Played
    {
        Home, Away
    }

    public static class Elo
    {
        private static double HOME_ADVANTAGE = 100.0d;
        public static (double expectedA, double expectedB) Probability(double ratingA, double ratingB)
        {
            var expectedScoreA = 1 / (1 + Math.Pow(10, (ratingB - ratingA) / 400));
            var expectedScoreB = 1 / (1 + Math.Pow(10, (ratingA - ratingB) / 400));
            return (expectedA: expectedScoreA, expectedB: expectedScoreB);
        }

        public static (double ratingA, double ratingB) Score(double ratingA, double ratingB, double scoreA, double scoreB, int kFactor=30)
        {
            var (expectedA, expectedB) = Probability(ratingA, ratingB);
            var newRatingA = ratingA + (kFactor * (scoreA - expectedA));
            var newRatingB = ratingB + (kFactor * (scoreB - expectedB));
            return (newRatingA, newRatingB);
        }

        public static (double ratingA, double ratingB) Score(double ratingA, double ratingB, Winner winner, int kFactor = 30) 
            => Score(ratingA, ratingB, winner == Winner.Player1 ? 1 : 0, winner == Winner.Player2 ? 1 : 0, kFactor: kFactor);

        public static (double ratingA, double ratingB) Score(double ratingA, double ratingB, Winner winner, Played playedPlayerA, int kFactor = 30)
            => Score(playedPlayerA == Played.Home ? ratingA + HOME_ADVANTAGE : ratingA, 
                playedPlayerA == Played.Away ? ratingB + HOME_ADVANTAGE : ratingB, 
                winner == Winner.Player1 ? 1 : 0, 
                winner == Winner.Player2 ? 1 : 0, 
                kFactor: kFactor);
    }
}
