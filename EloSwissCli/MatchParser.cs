using System.Collections.Generic;
using EloSwiss;
using System.Linq;

namespace EloSwissCli
{
    public class MatchParser
    {
        /// <summary>
        /// Convert '#A plays #B, #B wins' into Match object
        /// </summary>
        public static Tournament Parse(Tournament tournament, IEnumerable<EloSwissMatch> matches)
        {
            foreach (var match in matches.Select(x => x.AsMatch()))
            {
                var pair = tournament.Rounds
                    .SelectMany(rnd => rnd.Matches)
                    .Where(m => m.Player1.Name == match.Player1)
                    .Where(m => m.Player2.Name == match.Player2)
                    .FirstOrDefault();
                if (pair == null) continue;
                pair.Winner = match.Winner;
                pair.Score();
            }
            return tournament;
        }
    }
}