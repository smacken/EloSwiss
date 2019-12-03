using System;
using System.Collections.Generic;
using System.Linq;

namespace EloSwiss
{
    public class Swiss
    {
        public IEnumerable<Round> BuildRounds(Tournament tournament, IEnumerable<Round> previousRounds, int? activeRound = 1)
        {
            var roundSeed = new Random(activeRound ?? 1);
            var players = tournament.Players
                .Select(x => new {Player = x, Seed = roundSeed.Next()})
                .OrderBy(x => x.Player.Rating).ThenBy(x => x.Seed)
                .Select(x => x.Player).ToList();
            var matches = BuildMatchPairs(players);
            var previous = activeRound == 1
                ? Enumerable.Empty<Round>()
                : BuildRounds(tournament, null, activeRound - 1);
            return previous.Concat(new Round
            {
                Matches = matches.First().ToList(),
                Number = activeRound.GetValueOrDefault()
            });
        }

        public IEnumerable<IEnumerable<Match>> BuildMatchPairs(IList<Player> players)
        {
            if (players.Count % 2 == 1)
            {
                // create bye
            }
            else
            {
                for (var offset = 0; offset < players.Count / 2; offset++)
                {
                    var match = new Match {Player1 = players.ElementAt(offset), Player2 = players.ElementAt(offset+1)};
                    var subsequentMatches = BuildMatchPairs(players.Except(match.Players).ToList());
                    foreach (var subsequentMatch in subsequentMatches)
                        yield return new[] {match}.Concat(subsequentMatch);
                }
            }
        }
    }

    public class Tournament
    {
        public int Id { get; set; }
        public Guid Key { get; set; } = new Guid();
        public string Name { get; set; }
        public List<Player> Players { get; set; }
        public List<Round> Rounds { get; set; }
        public int ActiveRound { get; set; } = 1;
        public int RoundCount => (int)Math.Max(1, Math.Ceiling(Math.Log(Players.Count, 2)));
        public Round Round => Rounds.FirstOrDefault(x => x.Number == ActiveRound);
        // players should only play each-other once
        public bool IsValidMatch(Player player1, Player player2) =>
            !Rounds.SelectMany(round => round.Matches).Any(x => x.Players.Contains(player1) && x.Players.Contains(player2));
    }

    public class Round
    {
        public int Number { get; set; }
        public List<Match> Matches { get; set; }
    }

    public class Match
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Winner? Winner { get; set; }
        public void Score() => (Player1.Rating, Player2.Rating) = Elo.Score(Player1.Rating, Player2.Rating, Winner.Value);
        public (double rating1, double rating2) PredictedScore() => Elo.Probability(Player1.Rating, Player2.Rating);
        public List<Player> Players => new List<Player>(2) { Player1, Player2 };
        public Player PlayerWinner => Winner.HasValue && Winner.Value == EloSwiss.Winner.Player1 ? Player1 : Player2;
        public override string ToString() => Winner.HasValue ? $"#{Player1} vs #{Player2}" : $"#{Player1} vs #{Player2}, #{PlayerWinner} wins";
    }

    public class Player
    {
        public double Rating { get; set; }
        public string Name { get; set; }

        public Player(string name, double rating = 1000)
        {
            Name = name;
            Rating = rating;
        }
    }

    public class Standing
    {
        public readonly Player Player;
        public readonly int Rank;
        public readonly decimal MatchPoints;
        public readonly decimal OpponentsMatchWinPercentage;
        public readonly decimal GameWinPercentage;
        public readonly decimal OpponentsGameWinPercentage;

        public Standing(Player player, int rank, decimal matchPoints, decimal opponentsMatchWinPercentage, decimal gameWinPercentage, decimal opponentsGameWinPercentage)
        {
            Player = player;
            Rank = rank;
            MatchPoints = matchPoints;
            OpponentsMatchWinPercentage = opponentsMatchWinPercentage;
            GameWinPercentage = gameWinPercentage;
            OpponentsGameWinPercentage = opponentsGameWinPercentage;
        }

        public override string ToString() => $"#{Rank} - {Player}";
    }

    public static class Extensions
    {
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> collection, params T[] values)
        {
            if (collection == null)
                return null;

            if (values == null)
                return collection;

            return collection.Concat(values.AsEnumerable());
        }
    }
}