using System;

namespace ElectionCalc.Api.Models
{
    public class ShowScore
    {
        public string Id { get; set; }
        public string Party { get; set; }
        public double ScoreBatch1 { get; set; }
        public double ScoreBatch2 { get; set; }
        public double ScoreBatch3 { get; set; }
        public double ScoreBatch4 { get; set; }
        public double ScoreBatch5 { get; set; }
        public double ScoreBatch6 { get; set; }
        public double ScoreBatch7 { get; set; }
        public double ScoreBatch8 { get; set; }
        public double ScoreBatch9 { get; set; }
    }
}