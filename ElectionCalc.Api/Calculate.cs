using System.Collections.Generic;
using System.Text;
using ElectionCalc.Api.Models;

namespace ElectionCalc.Api
{
    public class Calculate
    {
        public List<string> Labels { get; set; }
        public List<string> Color = new List<string> { "#f53d3d", "#bdc3c7", "#ffce00", "#9b59b6", "#1abc9c", "#f39c12", "#3498db", "#F53D99" };

        public Calculate()
        {

        }

        public Calculate(List<ScoreAreaV2> scoreArea, ref List<DataSet> datasets)
        {
            Labels = new List<string>();

            foreach (var area in scoreArea)
            {
                var label = new StringBuilder();
                label.Append(area.Province).Append(" เขต ").Append(area.Zone);
                Labels.Add(label.ToString());
            }

            SetColor(ref datasets);
        }

        public void SetColor(ref List<DataSet> datasets)
        {
            for (int i = 0; i < datasets.Count; i++)
            {
                datasets[i].BackgroundColor = Color[i];
            }
        }

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

        public bool isSouth2(string province)
        {
            return province == "นครศรีธรรมราช"
                  || province == "กระบี่"
                  || province == "พังงา"
                  || province == "ภูเก็ต"
                  || province == "สุราษฎร์ธานี"
                  || province == "ระนอง"
                  || province == "ชุมพร"
                  || province == "สงขลา";
        }

        public bool isNorth(string province)
        {
            return province == "เชียงราย" || province == "เชียงใหม่" || province == "น่าน"
            || province == "พะเยา" || province == "แพร่" || province == "แม่ฮ่องสอน"
            || province == "ลำปาง" || province == "ลำพูน" || province == "อุตรดิตถ์"
            || province == "ตาก" || province == "พิษณุโลก" || province == "สุโขทัย"
            || province == "เพชรบูรณ์" || province == "พิจิตร" || province == "กำแพงเพชร"
            || province == "นครสวรรค์" || province == "อุทัยธานี";
        }

        public bool isNorthEast(string province)
        {
            return province == "กาฬสินธุ์" || province == "ขอนแก่น" || province == "ชัยภูมิ"
            || province == "นครพนม" || province == "นครราชสีมา" || province == "บึงกาฬ"
            || province == "บุรีรัมย์" || province == "มหาสารคาม" || province == "มุกดาหาร"
            || province == "ยโสธร" || province == "ร้อยเอ็ด" || province == "เลย"
            || province == "ศรีสะเกษ" || province == "สกลนคร" || province == "สุรินทร์"
            || province == "หนองคาย" || province == "หนองบัวลำภู" || province == "อำนาจเจริญ"
            || province == "อุดรธานี" || province == "อุบลราชธานี";
        }

        public bool isMiddle(string province)
        {
            return province == "กำแพงเพชร" || province == "ชัยนาท"
            || province == "นครนายก" || province == "นครปฐม" || province == "นครสวรรค์"
            || province == "นนทบุรี" || province == "ปทุมธานี" || province == "พระนครศรีอยุธยา"
            || province == "พิจิตร" || province == "พิษณุโลก" || province == "เพชรบูรณ์"
            || province == "ลพบุรี" || province == "สมุทรปราการ" || province == "สมุทรสงคราม"
            || province == "สมุทรสาคร" || province == "สระบุรี" || province == "สิงห์บุรี"
            || province == "สุโขทัย" || province == "สุพรรณบุรี" || province == "อ่างทอง"
            || province == "อุทัยธานี";
        }

        public bool isBKK(string province)
        {
            return province == "กรุงเทพมหานคร";
        }

        public bool isEast(string province)
        {
            return province == "จันทบุรี" || province == "ฉะเชิงเทรา"
            || province == "ชลบุรี" || province == "ตราด" || province == "ปราจีนบุรี"
            || province == "ระยอง" || province == "สระแก้ว";
        }

        public bool isWest(string province)
        {
            return province == "กาญจนบุรี" || province == "ตาก"
            || province == "ประจวบคีรีขันธ์" || province == "เพชรบุรี" || province == "ราชบุรี";
        }

        public string setRegion(string province)
        {
            if (isBKK(province))
            {
                return "กรุงเทพมหานคร";
            }
            if (isEast(province))
            {
                return "ตะวันออก";
            }
            if (isWest(province))
            {
                return "ตะวันตก";
            }
            if (isMiddle(province))
            {
                return "กลาง";
            }
            if (isNorth(province))
            {
                return "เหนือ";
            }
            if (isSouth2(province))
            {
                return "ใต้";
            }
            if (is19Zone(province))
            {
                return "ใต้(19เขต)";
            }
            if (isNorthEast(province))
            {
                return "ตะวันออกเฉียงเหนือ";
            }
            return "";
        }


    }
}