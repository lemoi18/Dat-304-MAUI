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
using MauiApp8.Services.Health;
using System.Collections.ObjectModel;

namespace MauiApp8.Services.GraphService
{
    internal class LineChartService : IChartService
    {
        private readonly IHealthService _healthService;

        public LineChartService(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [Obsolete]
        public ISeries[] GetSeries()
        {
            DateTimeOffset fromDate = DateTimeOffset.UtcNow.AddDays(-1);
            DateTimeOffset toDate = DateTimeOffset.UtcNow;
            var glucoses = _healthService.ReadGlucoses(fromDate, toDate);
            var insulins = _healthService.ReadInsulins(fromDate, toDate);

            var glucosevalues = new ObservableCollection<float>(glucoses.Select(g => g.Glucose));
            var insulinvalues = new ObservableCollection<double>(insulins.Select(i => i.Insulin));


            return new ISeries[]
            {
            new LineSeries<float>

            {
                Values = glucosevalues,
                GeometrySize = 30,
                GeometryFill = new SolidColorPaint(SKColors.AliceBlue),
                GeometryStroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 4 },
                Fill = new SolidColorPaint(SKColors.Blue.WithAlpha(45)),
                Name = "Glucose",
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 10 },
                ScalesYAt = 0,
                LegendShapeSize = 35,

            },
            new LineSeries<double>
            {
                Values = insulinvalues,
                GeometrySize = 30,
                GeometryFill = new SolidColorPaint(SKColors.MistyRose),
                GeometryStroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 4 },
                Fill = new SolidColorPaint(SKColors.Red.WithAlpha(45)),
                Name = "Insulin",
                Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 5 },
                ScalesYAt = 0,
                LegendShapeSize = 35,

            }
        };
        }
    }

}
