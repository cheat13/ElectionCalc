using System;
using System.Collections.Generic;

namespace ElectionCalc.Api.Models
{
    public class DataChart
    {
        public List<string> Labels { get; set; }
        public List<DataSet> Datasets { get; set; }
    }

    public class DataSet
    {
        public string Label { get; set; }
        public List<Data> Data { get; set; }
        public string BackgroundColor { get; set; }
    }

    public class Data
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}