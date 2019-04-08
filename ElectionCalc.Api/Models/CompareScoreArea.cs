namespace ElectionCalc.Api.Models
{
    public class CompareScoreArea
    {
        public string Id { get; set; }
        public string Province { get; set; }
        public string Zone { get; set; }
        public double ScoreBatch1st { get; set; }
        public double ScoreBatch2nd { get; set; }
        public double Diff { get; set; }
        public double PercentDiff { get; set; }
    }
}