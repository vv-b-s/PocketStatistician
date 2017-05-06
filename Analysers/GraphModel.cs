using Com.Syncfusion.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PocketStatistician
{
    public class GraphModel
    {
        public ObservableArrayList Data { set; get; }

        public GraphModel(double[] Xi, double[] Yi)
        {
            Data = new ObservableArrayList();
            for (int i = 0; i < Xi.Length; i++)
                Data.Add(new ChartDataPoint(Xi[i], Yi[i]));
        }
    }
}