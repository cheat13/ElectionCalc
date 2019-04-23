using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using CsvHelper;
using ElectionCalc.Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ElectionCalc.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MockDataController : ControllerBase
    {
        IMongoCollection<ScoreElection> ScoreElection { get; set; }
        IMongoCollection<ScoreArea> ScoreArea { get; set; }
        IMongoCollection<ScoreParty> ScoreParty { get; set; }
        IMongoCollection<ScoreParty> ScorePartyRatio { get; set; }
        IMongoCollection<DataCountVoter> CountVoter { get; set; }
        IMongoCollection<ScoreElectionV2> ScoreElectionV2 { get; set; }
        IMongoCollection<ScoreAreaV2> ScoreAreaV2 { get; set; }
        IMongoCollection<ScoreElectionV3> ScoreElectionV3 { get; set; }
        IMongoCollection<ScoreAreaV3> ScoreAreaV3 { get; set; }

        public MockDataController()
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl("mongodb://thesdev:The$election@moninter.southeastasia.cloudapp.azure.com/election?maxIdleTimeMS=150000&minPoolSize=2"));
            settings.SslSettings = new SslSettings()
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };
            var mongoClient = new MongoClient(settings);
            var database = mongoClient.GetDatabase("election");
            ScoreElection = database.GetCollection<ScoreElection>("ScoreElection");
            ScoreArea = database.GetCollection<ScoreArea>("ScoreArea");
            ScoreParty = database.GetCollection<ScoreParty>("ScoreParty");
            ScorePartyRatio = database.GetCollection<ScoreParty>("ScorePartyRatio");
            CountVoter = database.GetCollection<DataCountVoter>("CountVoter");
            ScoreElectionV2 = database.GetCollection<ScoreElectionV2>("ScoreElectionV2");
            ScoreAreaV2 = database.GetCollection<ScoreAreaV2>("ScoreAreaV2");
            ScoreElectionV3 = database.GetCollection<ScoreElectionV3>("ScoreElectionV3");
            ScoreAreaV3 = database.GetCollection<ScoreAreaV3>("ScoreAreaV3");
        }

        #region 
        [HttpGet]
        public List<ScoreElection> GetData()
        {
            // var listScoreBatch6 = ScoreElection.Find(it => it.Batch == "6"
            // && (it.Province == "ตรัง" || it.Province == "นราธิวาส" || it.Province == "ปัตตานี"
            // || it.Province == "พัทลุง" || it.Province == "ยะลา" || it.Province == "สตูล")).ToList();

            // var test = listScoreBatch6.GroupBy(it => it.Province).Sum(it => it.GroupBy(i => i.Zone).Count());

            var listScoreBatch6 = ScoreElection.Find(it => it.Batch == "6"
            && (it.Province != "ตรัง" && it.Province != "นราธิวาส" && it.Province != "ปัตตานี"
            && it.Province != "พัทลุง" && it.Province != "ยะลา" && it.Province != "สตูล")).ToList();

            // var test = listScoreBatch6.Where(it => it.FirstName == null).GroupBy(it => it.Party).Select(it => it.Key).ToList();
            var test = listScoreBatch6.Where(it => it.FirstName == null).ToList();

            return test;
        }

        [HttpGet]
        public List<ScoreElection> GetMockData()
        {
            var reader = new ReaderCsv();
            return reader.GetScoreElectionFromCSV();
        }

        [HttpGet]
        public List<ScoreElection> GetMockDataBatch6()
        {
            var reader = new ReaderCsv();
            return reader.GetScoreElectionBatch6();
        }

        [HttpGet]
        public List<int> CheckCountProvince()
        {
            var listScoreBatch6 = ScoreElection.Find(it => it.Batch == "6").ToList();
            var listScore = ScoreElection.Find(it => true).ToList();
            var countProBatch6 = listScoreBatch6.GroupBy(it => it.Province).Count();
            var countPro = listScore.GroupBy(it => it.Province).Count();
            var pro = new List<int> { countProBatch6, countPro };
            return pro;
        }

        [HttpGet]
        public List<int> CheckCountZone()
        {
            var listScoreBatch6 = ScoreElection.Find(it => it.Batch == "6").ToList();
            var listScore = ScoreElection.Find(it => true).ToList();

            var countProBatch6 = listScoreBatch6.GroupBy(it => it.Province).ToList();
            var countZoneBatch6 = countProBatch6.Sum(it => it.GroupBy(i => i.Zone).Count());

            var countPro = listScore.GroupBy(it => it.Province);
            var countZonePro = countPro.Sum(it => it.GroupBy(i => i.Zone).Count());

            var zone = new List<int> { countZoneBatch6, countZonePro };
            return zone;
        }

        [HttpGet]
        public List<string> CheckProvince()
        {
            var listScoreBatch6 = ScoreElection.Find(it => it.Batch == "6").ToList();
            var listScore = ScoreElection.Find(it => it.Batch != "6").ToList();

            var groupBatch6 = listScoreBatch6.GroupBy(it => it.Province);
            var groupProRaw = listScore.GroupBy(it => it.Province);

            var proBatch6 = groupBatch6.Select(it => it.Key).ToList();
            var proRaw = groupProRaw.Select(it => it.Key).ToList();

            var test = proBatch6.Where(it => !proRaw.Any(i => i == it)).ToList();

            return test;
        }

        [HttpGet]
        public List<int> CheckCountParty()
        {
            var listScoreBatch6 = ScoreElection.Find(it => it.Batch == "6").ToList();
            var listScore = ScoreElection.Find(it => true).ToList();

            var countPartyBatch6 = listScoreBatch6.GroupBy(it => it.Party).Count();
            var countParty = listScore.GroupBy(it => it.Party).Count();
            var pro = new List<int> { countPartyBatch6, countParty };

            return pro;
        }

        [HttpGet]
        public List<string> CheckParty()
        {
            var listScoreBatch6 = ScoreElection.Find(it => it.Batch == "6").ToList();
            var listScore = ScoreElection.Find(it => true).ToList();

            var groupBatch6 = listScoreBatch6.GroupBy(it => it.Party);
            var groupProRaw = listScore.GroupBy(it => it.Party);

            var partyBatch6 = groupBatch6.Select(it => it.Key).ToList();
            var partyRaw = groupProRaw.Select(it => it.Key).ToList();

            var test = partyRaw.Where(it => !partyBatch6.Any(i => i == it)).ToList();

            return test;
        }

        [HttpPost]
        public void MockData()
        {
            var reader = new ReaderCsv();
            var data = reader.GetScoreElectionFromCSV();
            ScoreElection.InsertMany(data);
        }

        [HttpPost]
        public void MockDataBatch6()
        {
            var reader = new ReaderCsv();
            var listScoreBatch6 = reader.GetScoreElectionBatch6();

            var listScore = ScoreElection.Find(it => true).ToList();

            foreach (var data in listScoreBatch6)
            {
                var politician = listScore.FirstOrDefault(i => i.Province == data.Province && i.Zone == data.Zone && i.Party == data.Party);
                if (politician != null)
                {
                    data.FirstName = politician.FirstName;
                    data.LastName = politician.LastName;
                }
            }

            ScoreElection.InsertMany(listScoreBatch6);
        }

        [HttpDelete]
        public void DeleteBatch6()
        {
            ScoreElection.DeleteMany(it => it.Batch == "6");
        }

        [HttpPost("{batch}")]
        public void MockDataScoreArea(string batch)
        {
            var dataScoreElection = ScoreElection.Find(it => it.Batch == batch).ToList();
            var listScoreArea = new List<ScoreArea>();
            var dataGroupByProvince = dataScoreElection.GroupBy(it => it.Province);
            foreach (var dataGroupProvince in dataGroupByProvince)
            {
                var dataGroupByZone = dataGroupProvince.GroupBy(it => it.Zone);
                foreach (var dataZone in dataGroupByZone)
                {
                    listScoreArea.Add(new ScoreArea
                    {
                        Id = Guid.NewGuid().ToString(),
                        Province = dataGroupProvince.Key,
                        Zone = dataZone.Key,
                        Batch = batch,
                        Score = dataZone.Sum(it => it.Score)
                    });
                }
            }
            ScoreArea.InsertMany(listScoreArea);
        }

        [HttpGet("{batch}")]
        public List<ScoreArea> GetScoreAreaWithBatch(string batch)
        {
            var dataScoreAreaWithBatch = ScoreArea.Find(it => it.Batch == batch).ToList();
            return dataScoreAreaWithBatch;
        }

        [HttpGet("{batch}")]
        public int GetCountScoreAreaWithBatch(string batch)
        {
            var countScoreAreaWithBatch = ScoreArea.Find(it => it.Batch == batch).ToList().Count();
            return countScoreAreaWithBatch;
        }

        [HttpGet("{batch}")]
        public int GetTotalScoreAreaWithBatch(string batch)
        {
            var dataScoreArea = ScoreArea.Find(it => it.Batch == batch).ToList();
            var totalScore = dataScoreArea.Sum(it => it.Score);
            return totalScore;
        }

        [HttpGet]
        public int GetTotalScoreBatch6()
        {
            var reader = new ReaderCsv();
            var listDataBatch6 = reader.GetScoreElectionBatch6();
            return listDataBatch6.Sum(it => it.Score);
        }

        [HttpPost]
        public void MockDataScoreParty()
        {
            var listScore = ScoreElection.Find(it => true).ToList();
            var listPartyName = listScore.GroupBy(it => it.Party).Select(it => it.Key).ToList();

            var listScoreParty = new List<ScoreParty>();

            for (int i = 0; i < 6; i++)
            {
                foreach (var name in listPartyName)
                {
                    var party = new ScoreParty
                    {
                        Id = Guid.NewGuid().ToString(),
                        Party = name,
                        Batch = (i + 1).ToString()
                    };
                    listScoreParty.Add(party);
                }
            }

            foreach (var party in listScoreParty)
            {
                party.Score = listScore.Where(it => it.Party == party.Party && it.Batch == party.Batch).Sum(it => it.Score);
            }

            ScoreParty.InsertMany(listScoreParty);
        }

        [HttpPost]
        public void MockDataScorePartyBatch7()
        {
            var calc = new Calculate();

            var listScoreBatch6 = ScoreElection.Find(it => it.Batch == "6"
            && !(calc.is19Zone(it.Province))).ToList();

            var listScoreBatch7 = listScoreBatch6.GroupBy(it => it.Party).Select(it =>
                new ScoreParty
                {
                    Id = Guid.NewGuid().ToString(),
                    Party = it.Key,
                    Batch = "7",
                    Score = it.Sum(i => i.Score)
                }).ToList();

            ScoreParty.InsertMany(listScoreBatch7);
        }

        [HttpPost]
        public void MockDataScorePartyBatch8()
        {
            var calc = new Calculate();
            var listScoreBatch6 = ScoreElection.Find(it => it.Batch == "6").ToList();
            var listScoreSouthBatch6 = listScoreBatch6.Where(it => calc.isSouth(it.Province)).ToList();

            var listScoreBatch8 = listScoreSouthBatch6.GroupBy(it => it.Party).Select(it =>
                new ScoreParty
                {
                    Id = Guid.NewGuid().ToString(),
                    Party = it.Key,
                    Batch = "8",
                    Score = it.Sum(i => i.Score)
                }).ToList();

            ScoreParty.InsertMany(listScoreBatch8);
        }

        [HttpPost]
        public void MockDataScorePartyBatch9()
        {
            var calc = new Calculate();

            var listScoreBatch6 = ScoreElection.Find(it => it.Batch == "6").ToList();
            var listScoreSouthBatch6 = listScoreBatch6.Where(it => !calc.is19Zone(it.Province) && calc.isSouth(it.Province)).ToList();

            var listScoreBatch9 = listScoreSouthBatch6.GroupBy(it => it.Party).Select(it =>
                new ScoreParty
                {
                    Id = Guid.NewGuid().ToString(),
                    Party = it.Key,
                    Batch = "9",
                    Score = it.Sum(i => i.Score)
                }).ToList();

            ScoreParty.InsertMany(listScoreBatch9);
        }

        [HttpPost]
        public void MockDataScoreAreaBatch9()
        {
            var listScoreBatch6 = ScoreArea.Find(it => it.Batch == "6").ToList();
            var calc = new Calculate();

            var listScoreBatch8 = listScoreBatch6.Select(it => new ScoreArea
            {
                Id = Guid.NewGuid().ToString(),
                Province = it.Province,
                Zone = it.Zone,
                Batch = "9",
                Score = calc.isSouth(it.Province) && !calc.is19Zone(it.Province) ? it.Score : 0,
            }).ToList();

            ScoreArea.InsertMany(listScoreBatch8);
        }

        #endregion

        [HttpPost]
        public void MockDataScorePartyRatio()
        {
            var listScoreParty = ScoreParty.Find(it => true).ToList();

            var listScoreBatch = listScoreParty.GroupBy(it => it.Batch)
            .Select(it => new
            {
                Batch = it.Key,
                TotalScore = it.Sum(i => i.Score)
            }).ToList();

            foreach (var party in listScoreParty)
            {
                party.Score *= (100.0 / listScoreBatch.FirstOrDefault(it => it.Batch == party.Batch).TotalScore);
            }

            ScorePartyRatio.InsertMany(listScoreParty);
        }

        [HttpPost]
        public void MockTaleCountVoter()
        {
            var readerCsv = new ReaderCsv();
            var dataCountVoter = readerCsv.MockDataCountVoter();
            var dataScoreArea = ScoreArea.Find(it => it.Batch == "6").ToList();
            foreach (var data in dataCountVoter)
            {
                var countRealVote = dataScoreArea.Find(it => it.Province == data.Province && it.Zone == data.Zone).Score;
                data.CountRealVoter = countRealVote;
            }
            CountVoter.InsertMany(dataCountVoter);
        }

        [HttpPost]
        public void WriteCountVoterToCsv()
        {
            var filePath = @"CountVoter.csv";
            var dataCountVoter = CountVoter.Find(it => true).ToList();
            using (var textWriter = new StreamWriter(filePath))
            {
                var writer = new CsvWriter(textWriter);
                writer.Configuration.Delimiter = ",";
                writer.WriteRecords(dataCountVoter);
            }
        }

        [HttpGet]
        public int GetCountDataTableCountVoter()
        {
            return CountVoter.Find(it => true).ToList().Count();
        }

        [HttpGet]
        public List<DataCountVoter> GetTableCountVoter()
        {
            return CountVoter.Find(it => true).ToList();
        }

        [HttpGet]
        public List<ScoreElection> GetDataBatch6()
        {
            return ScoreElection.Find(it => it.Batch == "6").ToList();
        }

        [HttpPost]
        public void MockNewData()
        {
            var dataElection = ScoreElection.Find(it => it.Batch == "6").ToList();
            var listGroupByProvince = dataElection.GroupBy(it => it.Province).ToList();
            var listLateScoreElection2 = new List<ScoreElectionV2>();
            var cal = new Calculate();
            foreach (var dataGroupProvince in listGroupByProvince)
            {
                var dataGroupByZone = dataGroupProvince.GroupBy(it => it.Zone).ToList();
                foreach (var dataGroupZone in dataGroupByZone)
                {
                    var totalScoreZone = dataGroupZone.Sum(it => it.Score);
                    var listLateScoreElection = dataGroupZone.Select(it => new ScoreElectionV2
                    {
                        Id = Guid.NewGuid().ToString(),
                        Province = it.Province,
                        Zone = it.Zone,
                        Party = it.Party,
                        FirstName = it.FirstName,
                        LastName = it.LastName,
                        Score = it.Score,
                        PercentScore = it.Score * 100.0 / totalScoreZone,
                        Region = cal.setRegion(it.Province)
                    }).ToList();
                    listLateScoreElection2.AddRange(listLateScoreElection);
                }
            }
            ScoreElectionV2.InsertMany(listLateScoreElection2);
        }

        [HttpGet]
        public int GetCountScoreElectionV2()
        {
            return ScoreElectionV2.Find(it => true).ToList().Count();
        }

        [HttpGet]
        public List<ScoreElectionV2> GetScoreElectionV2()
        {
            return ScoreElectionV2.Find(it => true).ToList();
        }

        [HttpDelete]
        public void DeleteScoreElectionV2()
        {
            ScoreElectionV2.DeleteMany(it => true);
        }

        [HttpPost]
        public void MockDataScoreAreaV2()
        {
            var dataScoreElectionV2 = ScoreElectionV2.Find(it => true).ToList();
            var readerCsv = new ReaderCsv();
            var dataAuthority = readerCsv.GetDataAuthority();
            var listScoreAreaV2 = new List<ScoreAreaV2>();
            foreach (var dataGroupProvince in dataScoreElectionV2.GroupBy(it => it.Province))
            {
                foreach (var dataGroupZone in dataGroupProvince.GroupBy(it => it.Zone))
                {
                    var totalScore = dataGroupZone.Sum(it => it.Score);
                    var totalCountAuthority = dataAuthority.FirstOrDefault(it => it.Province == dataGroupProvince.Key
                     && it.Zone == dataGroupZone.Key).Score;
                    var percent = totalScore * 100.0 / totalCountAuthority;
                    var getWinner = dataGroupZone.FirstOrDefault(it => it.Score == dataGroupZone.Max(i => i.Score)).Party;
                    listScoreAreaV2.Add(new ScoreAreaV2
                    {
                        Id = Guid.NewGuid().ToString(),
                        Province = dataGroupProvince.Key,
                        Zone = dataGroupZone.Key,
                        Score = totalScore,
                        CountAuthority = totalCountAuthority,
                        PercentScore = percent,
                        PartyWin = getWinner,
                        Region = dataGroupZone.FirstOrDefault().Region
                    });
                }
            }
            ScoreAreaV2.InsertMany(listScoreAreaV2);
        }

        [HttpGet]
        public List<ScoreAreaV2> GetScoreAreaV2()
        {
            return ScoreAreaV2.Find(it => true).ToList();
        }

        [HttpGet]
        public int GetCountScoreAreaV2()
        {
            return ScoreAreaV2.Find(it => true).ToList().Count;
        }

        [HttpDelete]
        public void DeleteScoreAreaV2()
        {
            ScoreAreaV2.DeleteMany(it => true);
        }

        [HttpPost]
        public void SortDataScoreAreaV2ByRegion()
        {
            //var filePath = @"DataScoreAreaV2.csv";
            var count = 0;
            var dataScoreAreaV2 = ScoreAreaV2.Find(it => true).ToList();
            var listDataScoreAreaNorthEast = dataScoreAreaV2.Where(it => it.Region == "อีสาน").ToList();
            var listDataScoreAreaNorth = dataScoreAreaV2.Where(it => it.Region == "เหนือ").ToList();
            var listDataScoreAreaEast = dataScoreAreaV2.Where(it => it.Region == "ตะวันออก").ToList();
            var listDataScoreAreaWest = dataScoreAreaV2.Where(it => it.Region == "ตะวันตก").ToList();
            var listDataScoreAreaMid = dataScoreAreaV2.Where(it => it.Region == "กลาง").ToList();
            var listDataScoreAreaBkk = dataScoreAreaV2.Where(it => it.Region == "กทม").ToList();
            var listDataScoreArea2WithSouth = dataScoreAreaV2.Where(it => it.Region == "ใต้").ToList();
            var listDataScoreArea2WithSouth19Zone = dataScoreAreaV2.Where(it => it.Region == "ใต้(19เขต)").ToList();
            var listSortData = new List<ScoreAreaV2>();
            foreach (var dataScoreArea in listDataScoreAreaNorthEast)
            {
                count += 1;
                dataScoreArea.NoArea = count.ToString();
                listSortData.Add(dataScoreArea);
            }
            foreach (var dataScoreArea in listDataScoreAreaNorth)
            {
                count += 1;
                dataScoreArea.NoArea = count.ToString();
                listSortData.Add(dataScoreArea);
            }
            foreach (var dataScoreArea in listDataScoreAreaEast)
            {
                count += 1;
                dataScoreArea.NoArea = count.ToString();
                listSortData.Add(dataScoreArea);
            }
            foreach (var dataScoreArea in listDataScoreAreaWest)
            {
                count += 1;
                dataScoreArea.NoArea = count.ToString();
                listSortData.Add(dataScoreArea);
            }
            foreach (var dataScoreArea in listDataScoreAreaMid)
            {
                count += 1;
                dataScoreArea.NoArea = count.ToString();
                listSortData.Add(dataScoreArea);
            }
            foreach (var dataScoreArea in listDataScoreAreaBkk)
            {
                count += 1;
                dataScoreArea.NoArea = count.ToString();
                listSortData.Add(dataScoreArea);
            }
            foreach (var dataScoreArea in listDataScoreArea2WithSouth)
            {
                count += 1;
                dataScoreArea.NoArea = count.ToString();
                listSortData.Add(dataScoreArea);
            }
            foreach (var dataScoreArea in listDataScoreArea2WithSouth19Zone)
            {
                count += 1;
                dataScoreArea.NoArea = count.ToString();
                listSortData.Add(dataScoreArea);
            }

            ScoreAreaV2.DeleteMany(it => true);
            ScoreAreaV2.InsertMany(listSortData);

            // using (var textWriter = new StreamWriter(filePath))
            // {
            //     var writer = new CsvWriter(textWriter);
            //     writer.Configuration.Delimiter = ",";
            //     writer.WriteRecords(listSortData);
            // }
        }

        [HttpGet]
        public List<ScoreElectionV3> GetScoreElectionV3()
        {
            return ScoreElectionV3.Find(it => true).ToList();
        }

        [HttpGet]
        public int CountScoreElectionV3()
        {
            return ScoreElectionV3.Find(it => true).ToList().Count;
        }

        [HttpPost]
        public void MockScoreElectionV3()
        {
            var listScoreElectionV3 = new List<ScoreElectionV3>();
            var count = 0;
            var dataScoreElectionV2 = ScoreElectionV2.Find(it => true).ToList();
            var listDataScoreAreaNorthEast = dataScoreElectionV2.Where(it => it.Region == "อีสาน").ToList();
            var listDataScoreAreaNorth = dataScoreElectionV2.Where(it => it.Region == "เหนือ").ToList();
            var listDataScoreAreaEast = dataScoreElectionV2.Where(it => it.Region == "ตะวันออก").ToList();
            var listDataScoreAreaWest = dataScoreElectionV2.Where(it => it.Region == "ตะวันตก").ToList();
            var listDataScoreAreaMid = dataScoreElectionV2.Where(it => it.Region == "กลาง").ToList();
            var listDataScoreAreaBkk = dataScoreElectionV2.Where(it => it.Region == "กทม").ToList();
            var listDataScoreArea2WithSouth = dataScoreElectionV2.Where(it => it.Region == "ใต้").ToList();
            var listDataScoreArea2WithSouth19Zone = dataScoreElectionV2.Where(it => it.Region == "ใต้(19เขต)").ToList();

            foreach (var dataGroupProvince in listDataScoreAreaNorthEast.GroupBy(it => it.Province))
            {
                foreach (var dataGroupZone in dataGroupProvince.GroupBy(it => it.Zone).ToList())
                {
                    count += 1;
                    var listData = dataGroupZone.Select(it => new ScoreElectionV3
                    {
                        Id = it.Id,
                        Province = it.Province,
                        Zone = it.Zone,
                        NoArea = count.ToString(),
                        Party = it.Party,
                        FirstName = it.FirstName,
                        LastName = it.LastName,
                        Score = it.Score,
                        PercentScore = it.PercentScore,
                        Region = it.Region,
                    }).ToList();
                    listScoreElectionV3.AddRange(listData);
                }
            }

            foreach (var dataGroupProvince in listDataScoreAreaNorth.GroupBy(it => it.Province))
            {
                foreach (var dataGroupZone in dataGroupProvince.GroupBy(it => it.Zone).ToList())
                {
                    count += 1;
                    var listData = dataGroupZone.Select(it => new ScoreElectionV3
                    {
                        Id = it.Id,
                        Province = it.Province,
                        Zone = it.Zone,
                        NoArea = count.ToString(),
                        Party = it.Party,
                        FirstName = it.FirstName,
                        LastName = it.LastName,
                        Score = it.Score,
                        PercentScore = it.PercentScore,
                        Region = it.Region,
                    }).ToList();
                    listScoreElectionV3.AddRange(listData);
                }
            }

            foreach (var dataGroupProvince in listDataScoreAreaEast.GroupBy(it => it.Province))
            {
                foreach (var dataGroupZone in dataGroupProvince.GroupBy(it => it.Zone).ToList())
                {
                    count += 1;
                    var listData = dataGroupZone.Select(it => new ScoreElectionV3
                    {
                        Id = it.Id,
                        Province = it.Province,
                        Zone = it.Zone,
                        NoArea = count.ToString(),
                        Party = it.Party,
                        FirstName = it.FirstName,
                        LastName = it.LastName,
                        Score = it.Score,
                        PercentScore = it.PercentScore,
                        Region = it.Region,
                    }).ToList();
                    listScoreElectionV3.AddRange(listData);
                }
            }

            foreach (var dataGroupProvince in listDataScoreAreaWest.GroupBy(it => it.Province))
            {
                foreach (var dataGroupZone in dataGroupProvince.GroupBy(it => it.Zone).ToList())
                {
                    count += 1;
                    var listData = dataGroupZone.Select(it => new ScoreElectionV3
                    {
                        Id = it.Id,
                        Province = it.Province,
                        Zone = it.Zone,
                        NoArea = count.ToString(),
                        Party = it.Party,
                        FirstName = it.FirstName,
                        LastName = it.LastName,
                        Score = it.Score,
                        PercentScore = it.PercentScore,
                        Region = it.Region,
                    }).ToList();
                    listScoreElectionV3.AddRange(listData);
                }
            }

            foreach (var dataGroupProvince in listDataScoreAreaMid.GroupBy(it => it.Province))
            {
                foreach (var dataGroupZone in dataGroupProvince.GroupBy(it => it.Zone).ToList())
                {
                    count += 1;
                    var listData = dataGroupZone.Select(it => new ScoreElectionV3
                    {
                        Id = it.Id,
                        Province = it.Province,
                        Zone = it.Zone,
                        NoArea = count.ToString(),
                        Party = it.Party,
                        FirstName = it.FirstName,
                        LastName = it.LastName,
                        Score = it.Score,
                        PercentScore = it.PercentScore,
                        Region = it.Region,
                    }).ToList();
                    listScoreElectionV3.AddRange(listData);
                }
            }

            foreach (var dataGroupProvince in listDataScoreAreaBkk.GroupBy(it => it.Province))
            {
                foreach (var dataGroupZone in dataGroupProvince.GroupBy(it => it.Zone).ToList())
                {
                    count += 1;
                    var listData = dataGroupZone.Select(it => new ScoreElectionV3
                    {
                        Id = it.Id,
                        Province = it.Province,
                        Zone = it.Zone,
                        NoArea = count.ToString(),
                        Party = it.Party,
                        FirstName = it.FirstName,
                        LastName = it.LastName,
                        Score = it.Score,
                        PercentScore = it.PercentScore,
                        Region = it.Region,
                    }).ToList();
                    listScoreElectionV3.AddRange(listData);
                }
            }

            foreach (var dataGroupProvince in listDataScoreArea2WithSouth.GroupBy(it => it.Province))
            {
                foreach (var dataGroupZone in dataGroupProvince.GroupBy(it => it.Zone).ToList())
                {
                    count += 1;
                    var listData = dataGroupZone.Select(it => new ScoreElectionV3
                    {
                        Id = it.Id,
                        Province = it.Province,
                        Zone = it.Zone,
                        NoArea = count.ToString(),
                        Party = it.Party,
                        FirstName = it.FirstName,
                        LastName = it.LastName,
                        Score = it.Score,
                        PercentScore = it.PercentScore,
                        Region = it.Region,
                    }).ToList();
                    listScoreElectionV3.AddRange(listData);
                }
            }

            foreach (var dataGroupProvince in listDataScoreArea2WithSouth19Zone.GroupBy(it => it.Province))
            {
                foreach (var dataGroupZone in dataGroupProvince.GroupBy(it => it.Zone).ToList())
                {
                    count += 1;
                    var listData = dataGroupZone.Select(it => new ScoreElectionV3
                    {
                        Id = it.Id,
                        Province = it.Province,
                        Zone = it.Zone,
                        NoArea = count.ToString(),
                        Party = it.Party,
                        FirstName = it.FirstName,
                        LastName = it.LastName,
                        Score = it.Score,
                        PercentScore = it.PercentScore,
                        Region = it.Region,
                    }).ToList();
                    listScoreElectionV3.AddRange(listData);
                }
            }
            ScoreElectionV3.InsertMany(listScoreElectionV3);
        }

        [HttpDelete]
        public void DeleteScoreElectionV3()
        {
            ScoreElectionV3.DeleteMany(it => true);
        }

        [HttpPost]
        public void MockScoreAreaV3()
        {
            var dataScoreAreaV2 = ScoreAreaV2.Find(it => true).ToList();
            var scoreElection24 = ScoreElection.Find(it => it.Batch == "2").ToList();
            var scoreElection25 = ScoreElection.Find(it => it.Batch == "5").ToList();
            var listScoreAreaV3 = new List<ScoreAreaV3>();
            foreach (var data in dataScoreAreaV2)
            {
                if (scoreElection24.Any(it => it.Province == data.Province && it.Zone == data.Zone))
                {
                    var score24 = scoreElection24.Where(it => it.Province == data.Province && it.Zone == data.Zone)
                    .Sum(it => it.Score);
                    var maxScore24 = scoreElection24.Where(it => it.Province == data.Province && it.Zone == data.Zone)
                    .ToList().Max(it => it.Score);
                    var partyWin24 = scoreElection24.FirstOrDefault(it => it.Score == maxScore24).Party;

                    var score25 = scoreElection25.Where(it => it.Province == data.Province && it.Zone == data.Zone)
                    .Sum(it => it.Score);
                    var maxScore25 = scoreElection25.Where(it => it.Province == data.Province && it.Zone == data.Zone).Max(it => it.Score);
                    var partyWin25 = scoreElection25.FirstOrDefault(it => it.Score == maxScore25).Party;

                    listScoreAreaV3.Add(new ScoreAreaV3
                    {
                        Id = data.Id,
                        Province = data.Province,
                        Zone = data.Zone,
                        NoArea = data.NoArea,
                        Score = data.Score,
                        CountAuthority = data.CountAuthority,
                        PercentScore = data.PercentScore,
                        PartyWin = data.PartyWin,
                        Region = data.Region,
                        Score24 = score24,
                        Score25 = score25,
                        PartyWin24 = partyWin24,
                        PartyWin25 = partyWin25,
                    });
                }
                else
                {
                    listScoreAreaV3.Add(new ScoreAreaV3
                    {
                        Id = data.Id,
                        Province = data.Province,
                        Zone = data.Zone,
                        NoArea = data.NoArea,
                        Score = data.Score,
                        CountAuthority = data.CountAuthority,
                        PercentScore = data.PercentScore,
                        PartyWin = data.PartyWin,
                        Region = data.Region,
                        Score24 = 0,
                        Score25 = 0,
                        PartyWin24 = null,
                        PartyWin25 = null,
                    });
                }
            }
            ScoreAreaV3.InsertMany(listScoreAreaV3);
        }

        [HttpGet]
        public List<ScoreAreaV3> GetScoreAreaV3()
        {
            return ScoreAreaV3.Find(it => true).ToList();
        }

        [HttpGet]
        public int CountScoreAreaV3()
        {
            return ScoreAreaV3.Find(it => true).ToList().Count;
        }

        [HttpDelete]
        public void DeleteScoreV3()
        {
            ScoreAreaV3.DeleteMany(it => true);
        }
    }
}
