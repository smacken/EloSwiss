using System.Linq;
using System.Text.RegularExpressions;
using EloSwiss;

namespace EloSwissCli
{
    /// <summary>
    /// Round 1:
    /// #1 plays #5, #1 wins
    /// </summary>
    public class EloMatch
    {
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public Winner Winner { get; set; }
        public Played PlayedAt { get; set; } = Played.Home;
    }

    public class EloSwissMatch
    {
        public string Players { get; set; }
        public string Winner { get; set; }

        public EloMatch AsMatch()
        {
            Regex regex = new Regex(@"(?<=\#)(.*?)(?=\ )"); 
            var matches = regex.Matches(Winner);
            if (matches.Count == 0) return null;
            var player1 = Players.Split(" plays ").First().TrimStart('#');
            return new EloMatch 
            {
                Player1 = player1,
                Player2 = Players.Split(" plays ").Last().TrimStart('#'),
                Winner = matches.First().Value == player1 
                    ? EloSwiss.Winner.Player1 
                    : EloSwiss.Winner.Player2
            };
        }
    }
}