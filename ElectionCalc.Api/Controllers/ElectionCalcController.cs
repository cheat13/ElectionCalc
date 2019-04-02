using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace ElectionCalc.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElectionCalcController : ControllerBase
    {
        // IMongoCollection<ScorePoll> ScorePollCollection { get; set; }

        public ElectionCalcController()
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl("mongodb://thesdev:The$election@moninter.southeastasia.cloudapp.azure.com/election?maxIdleTimeMS=150000&minPoolSize=2"));
            settings.SslSettings = new SslSettings()
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };
            var mongoClient = new MongoClient(settings);
            var database = mongoClient.GetDatabase("election");
            // ElectionCollection = database.GetCollection<ElectionModel>("Election");
        }
    }
}
