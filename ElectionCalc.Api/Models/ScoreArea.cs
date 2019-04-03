using System;

namespace ElectionCalc.Api.Models
{
    public class ScoreArea
    {
        public string Id { get; set; }
        public string Province { get; set; }
        public string Zone { get; set; }
        public string Batch { get; set; }
        public DateTime Date { get; set; }
        public int Score { get; set; }
    }
}