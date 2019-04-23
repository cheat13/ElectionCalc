using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
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
        IMongoCollection<ScoreElectionV2> ScoreElectionV2 { get; set; }
        IMongoCollection<ScoreAreaV2> ScoreAreaV2 { get; set; }
        IMongoCollection<ScoreElectionV3> ScoreElectionV3 { get; set; }
        IMongoCollection<ScoreAreaV3> ScoreAreaV3 { get; set; }

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
            ScoreElectionV2 = database.GetCollection<ScoreElectionV2>("ScoreElectionV2");
            ScoreAreaV2 = database.GetCollection<ScoreAreaV2>("ScoreAreaV2");
            ScoreElectionV3 = database.GetCollection<ScoreElectionV3>("ScoreElectionV3");
            ScoreAreaV3 = database.GetCollection<ScoreAreaV3>("ScoreAreaV3");
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
            return ScoreParty.Find(it => it.Batch == "8").ToList();
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
                ScoreBatch7 = it.FirstOrDefault(i => i.Batch == "7").Score,
                ScoreBatch8 = it.Any(i => i.Batch == "8") ? it.FirstOrDefault(i => i.Batch == "8").Score : 0,
                ScoreBatch9 = it.Any(i => i.Batch == "9") ? it.FirstOrDefault(i => i.Batch == "9").Score : 0,
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
                ScoreBatch7 = it.FirstOrDefault(i => i.Batch == "7").Score,
                ScoreBatch8 = it.Any(i => i.Batch == "8") ? it.FirstOrDefault(i => i.Batch == "8").Score : 0,
                ScoreBatch9 = it.Any(i => i.Batch == "9") ? it.FirstOrDefault(i => i.Batch == "9").Score : 0,
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

        [HttpGet]
        public List<ShowScoreArea> GetShowScoreArea()
        {
            var dataScoreArea = ScoreArea.Find(it => true).ToList();
            var dataGroupByProvince = dataScoreArea.GroupBy(it => it.Province).ToList();
            var listShowScoreArea = new List<ShowScoreArea>();
            foreach (var dataGroupProvince in dataGroupByProvince)
            {
                var dataGroupByZone = dataGroupProvince.GroupBy(it => it.Zone).ToList();
                foreach (var dataGroupZone in dataGroupByZone)
                {
                    var scoreBatch1 = (dataGroupZone.Any(it => it.Batch == "1")) ?
                    dataGroupZone.FirstOrDefault(it => it.Batch == "1").Score : 0;
                    var scoreBatch2 = (dataGroupZone.Any(it => it.Batch == "2")) ?
                    dataGroupZone.FirstOrDefault(it => it.Batch == "2").Score : 0;
                    var scoreBatch3 = (dataGroupZone.Any(it => it.Batch == "3")) ?
                    dataGroupZone.FirstOrDefault(it => it.Batch == "3").Score : 0;
                    var scoreBatch4 = (dataGroupZone.Any(it => it.Batch == "4")) ?
                    dataGroupZone.FirstOrDefault(it => it.Batch == "4").Score : 0;
                    var scoreBatch5 = (dataGroupZone.Any(it => it.Batch == "5")) ?
                    dataGroupZone.FirstOrDefault(it => it.Batch == "5").Score : 0;
                    var scoreBatch6 = (dataGroupZone.Any(it => it.Batch == "6")) ?
                    dataGroupZone.FirstOrDefault(it => it.Batch == "6").Score : 0;
                    var scoreBatch7 = (dataGroupZone.Any(it => it.Batch == "7")) ?
                    dataGroupZone.FirstOrDefault(it => it.Batch == "7").Score : 0;
                    var scoreBatch8 = (dataGroupZone.Any(it => it.Batch == "8")) ?
                    dataGroupZone.FirstOrDefault(it => it.Batch == "8").Score : 0;
                    var scoreBatch9 = (dataGroupZone.Any(it => it.Batch == "9")) ?
                    dataGroupZone.FirstOrDefault(it => it.Batch == "9").Score : 0;
                    listShowScoreArea.Add(new ShowScoreArea
                    {
                        Id = Guid.NewGuid().ToString(),
                        Province = dataGroupProvince.Key,
                        Zone = dataGroupZone.Key,
                        ScoreBatch1 = scoreBatch1,
                        ScoreBatch2 = scoreBatch2,
                        ScoreBatch3 = scoreBatch3,
                        ScoreBatch4 = scoreBatch4,
                        ScoreBatch5 = scoreBatch5,
                        ScoreBatch6 = scoreBatch6,
                        ScoreBatch7 = scoreBatch7,
                        ScoreBatch8 = scoreBatch8,
                        ScoreBatch9 = scoreBatch9,
                    });
                }
            }
            return listShowScoreArea;
        }

        [HttpGet("{batch1st}/{batch2nd}")]
        public List<CompareScoreArea> GetCompareScoreArea(string batch1st, string batch2nd)
        {
            var dataScoreArea = GetShowScoreArea();
            var calc = new Calculate();
            var listCompareScoreArea = new List<CompareScoreArea>();
            foreach (var data in dataScoreArea)
            {
                var scoreBatch1 = calc.getScoreAreaByBatch(data, batch1st);
                var scoreBatch2 = calc.getScoreAreaByBatch(data, batch2nd);
                var diff = scoreBatch2 - scoreBatch1;
                var percentDiff = (scoreBatch1 == 0) ? 0 : (scoreBatch2 - scoreBatch1) / scoreBatch1 * 100.0;
                listCompareScoreArea.Add(new CompareScoreArea
                {
                    Id = data.Id,
                    Province = data.Province,
                    Zone = data.Zone,
                    ScoreBatch1st = scoreBatch1,
                    ScoreBatch2nd = scoreBatch2,
                    Diff = diff,
                    PercentDiff = percentDiff
                });
            }
            return listCompareScoreArea;
        }

        [HttpGet("{batch}")]
        public int GetCountBathcScoreArea(string batch)
        {
            var dataScoreAreaBatch = ScoreArea.Find(it => it.Batch == batch).ToList().Count();
            return dataScoreAreaBatch;
        }

        [HttpGet("{batch}")]
        public List<ScoreArea> GetBathcScoreArea(string batch)
        {
            var dataScoreAreaBatch = ScoreArea.Find(it => it.Batch == batch).ToList();
            return dataScoreAreaBatch;
        }

        [HttpGet]
        public List<ScoreElectionV2> GetScoreElectionV2()
        {
            var dataScoreElectionV2 = ScoreElectionV2.Find(it => true).ToList();
            return dataScoreElectionV2;
        }

        [HttpGet]
        public List<ScoreAreaV2> GetScoreAreaV2()
        {
            return ScoreAreaV2.Find(it => true).ToList();
        }

        [HttpGet]
        public List<ScoreElectionV3> GetScoreElectionV3()
        {
            return ScoreElectionV3.Find(it => true).ToList();
        }

        [HttpGet("{party}/{chart}")]
        public DataChart GetDataChart(string party, string chart)
        {
            var scoreArea = ScoreAreaV3.Find(it => true).ToList();
            var datasets = new List<DataSet>();
            var calc = new Calculate(scoreArea);

            if (party == "ทั้งหมด")
            {
                var dataAll = scoreArea.GroupBy(it => it.Region).Select(it => new DataSet
                {
                    Label = it.Key,
                    Data = it.Select(i => new Data
                    {
                        X = Convert.ToDouble(i.NoArea),
                        Y = calc.GetDataYAxes(chart, i)
                    }).ToList(),
                }).ToList();
                datasets.AddRange(dataAll);
            }
            else
            {
                var dataPartyWin = new DataSet
                {
                    Label = party,
                    Data = scoreArea.Where(it => it.PartyWin == party).Select(it => new Data
                    {
                        X = Convert.ToDouble(it.NoArea),
                        Y = calc.GetDataYAxes(chart, it)
                    }).ToList()
                };
                var dataOthers = new DataSet
                {
                    Label = "อื่นๆ",
                    Data = scoreArea.Where(it => it.PartyWin != party).Select(it => new Data
                    {
                        X = Convert.ToDouble(it.NoArea),
                        Y = calc.GetDataYAxes(chart, it)
                    }).ToList()
                };
                datasets.Add(dataPartyWin);
                datasets.Add(dataOthers);
            }

            calc.SetColor(ref datasets);
            var dataChart = new DataChart { Labels = calc.Labels, Datasets = datasets };

            return dataChart;
        }

        [HttpGet("{party}")]
        public DataChart GetDataPartyChart(string party)
        {
            var scoreElection = ScoreElectionV3.Find(it => true).ToList();
            var datasets = new List<DataSet>();

            var dataAll = scoreElection.Where(it => it.Party == party).GroupBy(it => it.Region).Select(it => new DataSet
            {
                Label = it.Key,
                Data = it.Select(i => new Data { X = Convert.ToDouble(i.NoArea), Y = i.PercentScore }).ToList(),
            }).ToList();
            datasets.AddRange(dataAll);

            var scoreArea = ScoreAreaV3.Find(it => true).ToList();
            var calc = new Calculate(scoreArea);
            calc.SetColor(ref datasets);
            var dataPartyChart = new DataChart { Labels = calc.Labels, Datasets = datasets };

            return dataPartyChart;
        }
    }
}
