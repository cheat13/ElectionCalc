using System;

namespace ElectionCalc.Api.Models
{
    public class ScoreParty
    {
        public string Id { get; set; }
        public string Party { get; set; }
        public string Batch { get; set; }
        public DateTime Date { get; set; }
        public int Score { get; set; }
    }
}