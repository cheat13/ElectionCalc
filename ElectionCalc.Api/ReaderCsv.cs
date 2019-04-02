using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                var dataLine = readFromCSV.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var line in dataLine)
                {
                    var data = line.Split(',').ToList();
                    for (int i = 0; i < 5; i++)
                    {
                        Int32.TryParse(data[5 + i], out Int32 score);
                        var Politician = new ScoreElection
                        {
                            Id = Guid.NewGuid().ToString(),
                            Province = data[0],
                            Zone = data[1],
                            Party = data[2],
                            FirstName = data[3],
                            LastName = data[4],
                            Batch = (i + 1).ToString(),
                            Score = score,
                        };
                        listScore.Add(Politician);
                    }
                }
            }
        }
        return listScore;
    }
}