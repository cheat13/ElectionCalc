namespace ElectionCalc.Api.Models
{
    public class DataCountVoter
    {
        public string Id { get; set; }
        public string Province { get; set; }
        public string Zone { get; set; }
        public int CountVoter { get; set; }
        public int CountRealVoter { get; set; }
    }
}