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
                        PercentScore = it.Score * 100.0 / totalScoreZone
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
    }
}
