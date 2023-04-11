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
using Newtonsoft.Json.Linq;

namespace MauiApp8.Services.GraphService
{
    internal class LineChartService : IChartService
    {
        [Obsolete]
        public ISeries[] GetSeries()
        {
            return new ISeries[]
            {
            new LineSeries<int>
            {
                Values = new int[] { 2, 4, 5, 6, 5, 4, 5, 2, 4, 5, 6, 5, 4, 5, 2, 4, 5, 6, 5, 4, 5, 2, 4, 5, 6, 5, 4, 5, 2, 4, 5, 6, 5, 4, 5, 2, 4, 5, 6, 5, 4, 5, },
                GeometrySize = 30,
                GeometryFill = new SolidColorPaint(SKColors.AliceBlue),
                GeometryStroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 4 },
                Fill = new SolidColorPaint(SKColors.Blue.WithAlpha(45)),
                Name = "Insulin",
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 10 },
                ScalesYAt = 0,
                LegendShapeSize = 35,

            },
            new LineSeries<int>
            {
                Values = new int[] { 10, 4, 7, 2, 9, 1, 6, 8, 3, 5, 2, 7, 9, 3, 1, 5, 8, 6, 10, 4, 1, 5, 7, 9, 2, 10, 6, 4, 8, 3, 1, 5, 7, 9, 2, 10, 6, 4, 8, 3 },
                GeometrySize = 30,
                GeometryFill = new SolidColorPaint(SKColors.MistyRose),
                GeometryStroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 4 },
                Fill = new SolidColorPaint(SKColors.Red.WithAlpha(45)),
                Name = "Glucose",
                Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 5 },
                ScalesYAt = 0,
                LegendShapeSize = 35,

            }
        };
        }
    }

}
