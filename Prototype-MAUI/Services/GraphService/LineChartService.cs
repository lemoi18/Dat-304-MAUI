using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;

namespace MauiApp8.Services.GraphService
{
    internal class LineChartService: IChartService
    {
        public ISeries[] GetSeries()
        {

            return new ISeries[]
            {

            new LineSeries<int>
            {
                Values = new int[] { 2, 4, 5, 6, 5, 4, 5 },
                Fill = null,
                Name = "Insulin Levels"
            }
            };
        }
    }
}
