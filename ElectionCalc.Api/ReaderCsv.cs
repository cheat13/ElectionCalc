using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

    public List<ScoreElection3Year> GetScoreElection48()
    {
        var filePath = @"score48.csv";
        var listDataScore24 = new List<ScoreElection3Year>();
        var name = new StringBuilder();
        using (var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                var dataFromCsv = reader.ReadLine();
                var dataFromCsvSplitNewLine = dataFromCsv.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var dataLine in dataFromCsvSplitNewLine)
                {
                    var dataScoreElection = dataLine.Split(',').ToList();
                    var listName = dataScoreElection[2].Split().ToList();
                    var firstName = "";
                    var lastName = new StringBuilder();

                    for (int i = 0; i < listName.Count; i++)
                    {
                        if (i == 0)
                        {
                            firstName = listName[i].Substring(3, listName[i].Length - 3);
                        }
                        else if (listName[i] != "")
                        {
                            lastName.Append(listName[i]).Append(" ");
                        }
                    }
                    Int32.TryParse(dataScoreElection[5], out Int32 score48);
                    listDataScore24.Add(new ScoreElection3Year
                    {
                        Id = Guid.NewGuid().ToString(),
                        Province = dataScoreElection[0],
                        Zone = dataScoreElection[1],
                        Party = dataScoreElection[4],
                        FirstName = firstName,
                        LastName = lastName.ToString(),
                        Score = score48
                    });
                }
            }
        }
        return listDataScore24;
    }

    public List<ScoreElection3Year> GetScoreElection50()
    {
        var filePath = @"score50.csv";
        var listDataScore50 = new List<ScoreElection3Year>();
        var name = new StringBuilder();
        using (var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                var dataFromCsv = reader.ReadLine();
                var dataFromCsvSplitNewLine = dataFromCsv.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var dataLine in dataFromCsvSplitNewLine)
                {
                    var dataScoreElection = dataLine.Split(',').ToList();
                    var listName = dataScoreElection[2].Split().ToList();
                    var firstName = "";
                    var lastName = new StringBuilder();

                    for (int i = 0; i < listName.Count; i++)
                    {
                        if (i == 0)
                        {
                            firstName = listName[i].Substring(3, listName[i].Length - 3);
                        }
                        else if (listName[i] != "")
                        {
                            lastName.Append(listName[i]).Append(" ");
                        }
                    }
                    Int32.TryParse(dataScoreElection[5], out Int32 score50);
                    listDataScore50.Add(new ScoreElection3Year
                    {
                        Id = Guid.NewGuid().ToString(),
                        Province = dataScoreElection[0],
                        Zone = dataScoreElection[1],
                        Party = dataScoreElection[4],
                        FirstName = firstName,
                        LastName = lastName.ToString(),
                        Score = score50
                    });
                }
            }
        }
        return listDataScore50;
    }

    public List<ScoreElection3Year> GetScoreElection54()
    {
        var filePath = @"score54.csv";
        var listDataScore54 = new List<ScoreElection3Year>();
        var name = new StringBuilder();
        using (var reader = new StreamReader(filePath))
        {
            while (!reader.EndOfStream)
            {
                var dataFromCsv = reader.ReadLine();
                var dataFromCsvSplitNewLine = dataFromCsv.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach (var dataLine in dataFromCsvSplitNewLine)
                {
                    var dataScoreElection = dataLine.Split(',').ToList();
                    var listName = dataScoreElection[2].Split().ToList();
                    var firstName = "";
                    var lastName = new StringBuilder();

                    for (int i = 0; i < listName.Count; i++)
                    {
                        if (i == 0)
                        {
                            firstName = listName[i].Substring(3, listName[i].Length - 3);
                        }
                        else if (listName[i] != "")
                        {
                            lastName.Append(listName[i]).Append(" ");
                        }
                    }
                    Int32.TryParse(dataScoreElection[5], out Int32 score50);
                    listDataScore54.Add(new ScoreElection3Year
                    {
                        Id = Guid.NewGuid().ToString(),
                        Province = dataScoreElection[0],
                        Zone = dataScoreElection[1],
                        Party = dataScoreElection[4],
                        FirstName = firstName,
                        LastName = lastName.ToString(),
                        Score = score50
                    });
                }
            }
        }
        return listDataScore54;
    }

}

