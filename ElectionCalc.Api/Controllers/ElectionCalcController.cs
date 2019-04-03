using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using ElectionCalc.Api.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ElectionCalc.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ElectionCalcController : ControllerBase
    {
        IMongoCollection<ScoreElection> ScoreElection { get; set; }
        IMongoCollection<ScoreArea> ScoreArea { get; set; }
        IMongoCollection<ScoreParty> ScoreParty { get; set; }

        public ElectionCalcController()
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
        }

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

        [HttpPost]
        public void MockDataScoreArea()
        {

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
    }
}
