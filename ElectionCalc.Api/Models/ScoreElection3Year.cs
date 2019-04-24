using System;

namespace ElectionCalc.Api.Models
{
    public class ScoreElection3Year
    {
        public string Id { get; set; }
        public string Province { get; set; }
        public string Zone { get; set; }
        public string Party { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Score { get; set; }
    }
}