namespace ElectionCalc.Api.Models
{
    public class CompareScore
    {
        public string Id { get; set; }
        public string Party { get; set; }
        public double ScoreBatch1st { get; set; }
        public double ScoreBatch2nd { get; set; }
        public double diff { get; set; }
        public double percentDiff { get; set; }
    }
}