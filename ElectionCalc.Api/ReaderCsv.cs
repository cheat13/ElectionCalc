using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Election.Api.Models;
using ElectionCalc.Api.Models;

public class ReaderCsv
{
    public List<ScoreElection> GetScoreElectionFromCSV()
    {
        var pathFile = @"RawData.csv";
        var listScore = new List<ScoreElection>();
        using (var reader = new StreamReader(pathFile))
        {
            while (!reader.EndOfStream)
            {
                var readFromCSV = reader.ReadLine();
                var dataLine = readFromCSV.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList().Skip(0);
                foreach (var line in dataLine)
                {
                    var data = line.Split(',').ToList();
                    var Politician = new ScoreElection
                    {
                        Province = data[0],
                        Zone = data[1],
                        Party = data[2],
                        FirstName = data[3],
                        LastName = data[4],
                    };
                    for (int i = 0; i < 5; i++)
                    {
                        Politician.Id = Guid.NewGuid().ToString();
                        Politician.Batch = i + 1;
                        Politician.Score = int.Parse(data[5 + i]);
                        listScore.Add(Politician);
                    }
                }
            }
        }
        return listScore;
    }
}