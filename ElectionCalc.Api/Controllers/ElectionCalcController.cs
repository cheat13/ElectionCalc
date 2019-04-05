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
        IMongoCollection<ScoreParty> ScorePartyRatio { get; set; }

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
            ScorePartyRatio = database.GetCollection<ScoreParty>("ScorePartyRatio");
        }

        [HttpGet]
        public List<ScoreElection> GetScoreElection()
        {
            return ScoreElection.Find(it => true).ToList();
        }

        [HttpGet]
        public List<ScoreArea> GetScoreArea()
        {
            return ScoreArea.Find(it => true).ToList();
        }

        [HttpGet]
        public List<ScoreParty> GetScoreParty()
        {
            return ScoreParty.Find(it => true).ToList();
        }

        [HttpGet]
        public List<ScoreParty> GetScorePartyRatio()
        {
            return ScorePartyRatio.Find(it => true).ToList();
        }

        [HttpGet]
        public List<ShowScore> GetShowScoreParty()
        {
            var scoreParty = ScoreParty.Find(it => true).ToList();
            var showScoreParty = scoreParty.GroupBy(it => it.Party).Select(it => new ShowScore
            {
                Id = Guid.NewGuid().ToString(),
                Party = it.Key,
                ScoreBatch1 = it.FirstOrDefault(i => i.Batch == "1").Score,
                ScoreBatch2 = it.FirstOrDefault(i => i.Batch == "2").Score,
                ScoreBatch3 = it.FirstOrDefault(i => i.Batch == "3").Score,
                ScoreBatch4 = it.FirstOrDefault(i => i.Batch == "4").Score,
                ScoreBatch5 = it.FirstOrDefault(i => i.Batch == "5").Score,
                ScoreBatch6 = it.FirstOrDefault(i => i.Batch == "6").Score,
            }).OrderByDescending(it => it.ScoreBatch6).ToList();

            return showScoreParty;
        }

        [HttpGet]
        public List<ShowScore> GetShowScorePartyRatio()
        {
            var scoreParty = ScorePartyRatio.Find(it => true).ToList();
            var showScorePartyRatio = scoreParty.GroupBy(it => it.Party).Select(it => new ShowScore
            {
                Id = Guid.NewGuid().ToString(),
                Party = it.Key,
                ScoreBatch1 = it.FirstOrDefault(i => i.Batch == "1").Score,
                ScoreBatch2 = it.FirstOrDefault(i => i.Batch == "2").Score,
                ScoreBatch3 = it.FirstOrDefault(i => i.Batch == "3").Score,
                ScoreBatch4 = it.FirstOrDefault(i => i.Batch == "4").Score,
                ScoreBatch5 = it.FirstOrDefault(i => i.Batch == "5").Score,
                ScoreBatch6 = it.FirstOrDefault(i => i.Batch == "6").Score,
            }).OrderByDescending(it => it.ScoreBatch6).ToList();

            return showScorePartyRatio;
        }

        [HttpGet("{batch1st}/{batch2nd}")]
        public List<CompareScore> GetCompareScoreParty(string batch1st, string batch2nd)
        {
            var scoreParty = GetShowScoreParty();
            var listCompareScoreParty = new List<CompareScore>();
            var calc = new Calculate();
            foreach (var party in scoreParty)
            {
                var scoreBatch1st = calc.getScoreByBatch(party, batch1st);
                var scoreBatch2nd = calc.getScoreByBatch(party, batch2nd);
                var compareScoreParty = new CompareScore()
                {
                    Id = party.Id,
                    Party = party.Party,
                    ScoreBatch1st = scoreBatch1st,
                    ScoreBatch2nd = scoreBatch2nd,
                    diff = scoreBatch2nd - scoreBatch1st,
                    percentDiff = (scoreBatch1st == 0) ? 0 : (scoreBatch2nd - scoreBatch1st) / scoreBatch1st * 100,
                };
                listCompareScoreParty.Add(compareScoreParty);
            }
            return listCompareScoreParty;
        }

        [HttpGet("{batch1st}/{batch2nd}")]
        public List<CompareScore> GetCompareScorePartyRatio(string batch1st, string batch2nd)
        {
            var scorePartyRatio = GetShowScorePartyRatio();
            var listCompareScorePartyRatio = new List<CompareScore>();
            var calc = new Calculate();
            foreach (var party in scorePartyRatio)
            {
                var scoreBatch1st = calc.getScoreByBatch(party, batch1st);
                var scoreBatch2nd = calc.getScoreByBatch(party, batch2nd);
                var compareScoreParty = new CompareScore()
                {
                    Id = party.Id,
                    Party = party.Party,
                    ScoreBatch1st = scoreBatch1st,
                    ScoreBatch2nd = scoreBatch2nd,
                    diff = scoreBatch2nd - scoreBatch1st,
                    percentDiff = (scoreBatch1st == 0) ? 0 : (scoreBatch2nd - scoreBatch1st) / scoreBatch1st * 100,
                };
                listCompareScorePartyRatio.Add(compareScoreParty);
            }
            return listCompareScorePartyRatio;
        }

    }
}
