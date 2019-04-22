using System;

namespace ElectionCalc.Api.Models
{
    public class ScoreAreaV3
    {
        public string Id { get; set; }
        public string Province { get; set; }
        public string Zone { get; set; }
        public string NoArea { get; set; }
        public int Score { get; set; }
        public int CountAuthority { get; set; }
        public double PercentScore { get; set; }
        public string PartyWin { get; set; }
        public string Region { get; set; }
        public int Score24 { get; set; }
        public int Score25 { get; set; }
        public string PartyWin24 { get; set; }
        public string PartyWin25 { get; set; }
    }
}