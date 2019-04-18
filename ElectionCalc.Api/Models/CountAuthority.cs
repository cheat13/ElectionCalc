using System;

namespace ElectionCalc.Api.Models
{
    public class CountAuthority
    {
        public string Id { get; set; }
        public string Province { get; set; }
        public string Zone { get; set; }
        public int Score { get; set; }
    }
}