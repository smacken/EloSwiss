using System;
using System.Collections.Generic;
using System.Linq;

namespace EloSwiss
{
    public class Swiss
    {
        public IEnumerable<Round> BuildRounds(Tournament tournament, 
            IEnumerable<Round> previousRounds, 
            int? activeRound = 1)
        {
            var roundSeed = new Random(activeRound ?? 1);
            var players = tournament.Players
                .Select(player => new {
                    Player = player, 
                    Seed = player.Seed ?? roundSeed.Next()
                })
                .OrderBy(x => x.Player.Rating).ThenBy(x => x.Seed)
                .Select(x => x.Player).ToList();
            var matches = BuildMatchPairs(players, tournament);
            var previous = activeRound == 1
                ? Enumerable.Empty<Round>()
                : BuildRounds(tournament, null, activeRound - 1);
            return previous.Concat(new Round
            {
                Matches = matches.First().ToList(),
                Number = activeRound.GetValueOrDefault()
            });
        }

        public IEnumerable<IEnumerable<Match>> BuildMatchPairs(IList<Player> players, Tournament tournament)
        {
            if (players.Count % 2 == 1)
            {
                // create bye
                var byePlayer = players.Where(p => !p.HadBye).OrderBy(p => p.Rating).FirstOrDefault();
                var byeMatch = new ByeMatch(byePlayer);
                var remaining = players.Except(new List<Player> {byePlayer}).ToList();
                var matches = new List<Match>{byeMatch};
                foreach (var subsequentMatch in BuildMatchPairs(remaining, tournament))
                    matches.AddRange(subsequentMatch);
                yield return matches;
            }
            else
            {
                for (var offset = 0; offset < players.Count / 2; offset++)
                {
                    int offsetInc = 1;
                    bool validMatch = false;
                    Match match = null;
                    while (!validMatch)
                    {
                        match = new Match { 
                            Player1 = players.ElementAt(offset), 
                            Player2 = players.ElementAt(offset + offsetInc) 
                        };
                        validMatch = tournament.IsValidMatch(match.Player1, match.Player2);
                        offsetInc++;     
                    }
                    
                    var remaining = players.Except(match.Players).ToList();
                    var matches = new List<Match> { match };
                    foreach (var subsequentMatch in BuildMatchPairs(remaining, tournament))
                        matches.AddRange(subsequentMatch);
                    yield return matches;
                }
            }
        }

        public IEnumerable<Match> BuildPlayoffRound(Tournament tournament, PlayoffRound? round)
        {
            int playoffCuttoff = round.Cutoff(tournament.Players.Count);
            var players = tournament.Players
                .Take(playoffCuttoff)
                .ToList();
            var playerCount = players.Count;
            players.ForEach(p => p.Rating += 1000);
            for (var i=0; i < playerCount /2; i++)
                yield return new Match { 
                    Player1 = players.ElementAt(i), 
                    Player2 = players.ElementAt(playerCount-1-i)
                };
        }
    }

    public class Tournament
    {
        public int Id { get; set; }
        public Guid Key { get; set; } = new Guid();
        public string Name { get; set; }
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Round> Rounds { get; set; } = new List<Round>();
        public List<Standing> PlayerStandings { get; set; } = new List<Standing>();
        public int ActiveRound { get; set; } = 1;
        public int RoundCount => (int)Math.Max(1, Math.Ceiling(Math.Log(Players.Count, 2)));
        public Round Round => Rounds.FirstOrDefault(x => x.Number == ActiveRound);
        // players should only play each-other once
        public bool IsValidMatch(Player player1, Player player2) =>
            !Rounds.SelectMany(round => round.Matches).Any(x => x.Players.Contains(player1) && x.Players.Contains(player2));
        public List<Player> Opponents(Player player) => Rounds.SelectMany(r => r.Opponents(player)).ToList();
        public List<Standing> CurrentStandings() 
        {
            PlayerStandings = Players.Select(player => new Standing(this, player)).ToList();
            return PlayerStandings.OrderBy(x => x.Rank).ToList();
        } 
    }

    public class Round
    {
        public int Number { get; set; }
        public List<Match> Matches { get; set; } = new List<Match>();
        public override string ToString() => $"Round {Number}";
        public List<Player> Opponents(Player player) => Matches
            .Where(p => p.Players.Contains(player))
            .SelectMany(p => p.Players)
            .Where(a => a != player)
            .ToList();
        
        public List<Player> Winners => Matches.Select(match => match.PlayerWinner).ToList();
    }

    public class Match
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Winner? Winner { get; set; }
        public Player Home { get; set; }
        public bool IsBye => Player1 == null || Player2 == null;
        public void Score() => (Player1.Rating, Player2.Rating) = Elo.Score(Player1.Rating, Player2.Rating, Winner.Value);
        public (double rating1, double rating2) PredictedScore() => Elo.Probability(Player1.Rating, Player2.Rating);
        public List<Player> Players => new List<Player>(2) { Player1, Player2 };
        public Player PlayerWinner => !Winner.HasValue ? null : Winner.Value == EloSwiss.Winner.Player1 ? Player1 : Player2;
        public override string ToString() => Winner.HasValue 
            ? $"#{Player1.Name} vs #{Player2.Name}, #{PlayerWinner.Name} wins" 
            : $"#{Player1.Name} vs #{Player2.Name}";
    }

    public class ByeMatch : Match
    {
        private double _byeScore;
        public ByeMatch(Player player)
        {
            Player1 = player;
            Winner = EloSwiss.Winner.Player1;
        }

        public new void Score() => (this.Player1.Rating, _byeScore) = Elo.Score(Player1.Rating, 1000, EloSwiss.Winner.Player1);
        public override string ToString() => $"#{Player1.Name} Bye";
    }

    public class Player
    {
        private const int DefaultRating = 1000;
        public double Rating { get; set; }
        public string Name { get; set; }
        public bool HadBye { get; set; }
        public int? Seed { get; set; }

        public Player(string name, double rating = DefaultRating)
        {
            Name = name;
            Rating = rating;
        }

        public override string ToString() => $"{Name} ({Rating})";
    }

    public class Standing
    {
        private Tournament _tournament;
        public readonly Player Player;
        public int Rank => _tournament.Players
            .OrderByDescending(player => player.Rating)
            .ToList()
            .IndexOf(Player) + 1;
        public decimal MatchPoints => _tournament.Rounds
            .SelectMany(round => round.Matches)
            .Where(match => match.Players.Contains(Player))
            .Where(match => match.PlayerWinner == Player)
            .Count();
        public decimal GameWinPercentage => 
            (_tournament.Rounds.Count(x => x.Winners.Contains(Player)) / _tournament.RoundCount) * 100;
        
        public Standing(Tournament tournament, Player player)
        {
            _tournament = tournament;
            Player = player;
        }

        public override string ToString() => $"#{Rank} - {Player}";
    }

    public enum PlayoffRound
    {
        Final=2, SemiFinal=4, QuarterFinal=8, Playoff=16
    }

    public static class PlayoffRoundExtensions
    {
        public static int Cutoff(this PlayoffRound? round, int playerCount)
        {
            // number of players could be < round val
            bool accelerate = round.HasValue && playerCount < (int)round;
            int cutoff;

            if (accelerate)
            {
                cutoff = Enum.GetValues(typeof(PlayoffRound))
                    .Cast<int>()
                    .Where(x => x < playerCount)
                    .Max();
            } else 
            {
                cutoff = round.HasValue ? (int)round : 16;
            }
            return cutoff;
        }
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