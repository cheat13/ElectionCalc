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

    public List<ScoreElection> GetScoreElectionBatch6()
    {
        var pathFile = @"Batch6.csv";
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
                    Int32.TryParse(data[3], out Int32 score);
                    var Politician = new ScoreElection
                    {
                        Id = Guid.NewGuid().ToString(),
                        Province = data[0],
                        Zone = data[1],
                        Party = data[2],
                        // FirstName = data[3],
                        // LastName = data[4],
                        Batch = "6",
                        Score = score,
                    };
                    listScore.Add(Politician);
                }
            }
        }
        return listScore;
    }

    public List<DataCountVoter> MockDataCountVoter()
    {
        var filePath = @"DataScoreArea.csv";
        var listCountVoter = new List<DataCountVoter>();
        using (var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                var readFromCSV = reader.ReadLine();
                var listDataLine = readFromCSV.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var dataLine in listDataLine)
                {
                    var dataCountVoter = dataLine.Split(',').ToList();
                    Int32.TryParse(dataCountVoter[2], out Int32 countVoter);
                    listCountVoter.Add(new DataCountVoter
                    {
                        Id = Guid.NewGuid().ToString(),
                        Province = dataCountVoter[0],
                        Zone = dataCountVoter[1],
                        CountVoter = countVoter,
                        CountRealVoter = 0
                    });
                }
            }
        }
        return listCountVoter;
    }

    public List<CountAuthority> GetDataAuthority()
    {
        var filePath = @"authority.csv";
        var listDataAuthority = new List<CountAuthority>();
        using (var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                var readFromCSV = reader.ReadLine();
                var listDataLine = readFromCSV.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var dataLine in listDataLine)
                {
                    var dataAuthority = dataLine.Split(',').ToList();
                    Int32.TryParse(dataAuthority[2], out Int32 CountAuthority);
                    listDataAuthority.Add(new CountAuthority
                    {
                        Id = Guid.NewGuid().ToString(),
                        Province = dataAuthority[0],
                        Zone = dataAuthority[1],
                        Score = CountAuthority
                    });
                }
            }
        }
        return listDataAuthority;
    }

}