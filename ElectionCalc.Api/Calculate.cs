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
                case "7": return party.ScoreBatch7;
                case "8": return party.ScoreBatch8;
                case "9": return party.ScoreBatch9;
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
                case "7": return area.ScoreBatch7;
                case "8": return area.ScoreBatch8;
                case "9": return area.ScoreBatch9;
                default: return 0;
            }
        }

        public bool is19Zone(string province)
        {
            return province == "ตรัง"
                  || province == "นราธิวาส"
                  || province == "ปัตตานี"
                  || province == "พัทลุง"
                  || province == "ยะลา"
                  || province == "สตูล";
        }

        public bool isSouth(string province)
        {
            return is19Zone(province) || province == "นครศรีธรรมราช"
                  || province == "กระบี่"
                  || province == "พังงา"
                  || province == "ภูเก็ต"
                  || province == "สุราษฎร์ธานี"
                  || province == "ระนอง"
                  || province == "ชุมพร"
                  || province == "สงขลา";
        }
    }
}