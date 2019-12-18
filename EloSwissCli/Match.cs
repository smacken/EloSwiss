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
        public string Winner { get; set; }
    }

    public class EloSwissMatch
    {
        public string Players { get; set; }
        public string Winner { get; set; }
    }
}