using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp8.Services.GraphService
{
    internal class LineChartService: IChartService
    {
        public ISeries[] GetSeries()
        {

            return new ISeries[]
            {

            new LineSeries<double>
            {
                Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                Fill = null
            }
            };
        }
    }
}
