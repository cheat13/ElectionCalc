using ElectionCalc.Api.Models;

namespace ElectionCalc.Api
{
    public class Calculate
    {
        public double getScoreByBatch(ShowScore party, string batch)
        {
            switch (batch)
            {
                case "1": return party.ScoreBatch1;
                case "2": return party.ScoreBatch2;
                case "3": return party.ScoreBatch3;
                case "4": return party.ScoreBatch4;
                case "5": return party.ScoreBatch5;
                case "6": return party.ScoreBatch6;
                default: return 0;
            }
        }

        public double getScoreAreaByBatch(ShowScoreArea area, string batch)
        {
            switch (batch)
            {
                case "1": return area.ScoreBatch1;
                case "2": return area.ScoreBatch2;
                case "3": return area.ScoreBatch3;
                case "4": return area.ScoreBatch4;
                case "5": return area.ScoreBatch5;
                case "6": return area.ScoreBatch6;
                default: return 0;
            }
        }


    }
}