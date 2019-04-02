using System;
using MongoDB.Bson.Serialization.Attributes;
namespace ElectionCalc.Api.Models
{
    public class ScoreElection
    {
        [BsonId]
        public string Id { get; set; }
        public string Province { get; set; }
        public string Zone { get; set; }
        public string Party { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Batch { get; set; }
        public DateTime Date { get; set; }
        public int Score { get; set; }
    }
}