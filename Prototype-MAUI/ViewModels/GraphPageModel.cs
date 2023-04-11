using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MauiApp8.Services.GraphService;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting.Effects;

namespace MauiApp8.ViewModel
{
    public partial class GraphPageModel : ObservableObject
    {
        private int XMax { get; set; }
        private readonly IChartService _chartService;
        [ObservableProperty]
        private ISeries[] _series;

        public GraphPageModel(IChartService chartService)
        {
            _chartService = chartService;
            _series = _chartService.GetSeries();
            XMax = ((LineSeries<int>)_series[0]).Values.Count();
            Title = new LabelVisual
            {
                Text = "Insulin Health Data",
                TextSize = 72,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };

            XAxes = new Axis[]
{
                new Axis
                {
                    Name = "Time",
                    NameTextSize = 50,
                    MinLimit = (XMax-10),
                    MaxLimit = XMax,
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    MinStep = 1,

                    LabelsPaint = new SolidColorPaint(SKColors.Blue),
                    TextSize = 72,

                    SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 1 }
                }
 };
            YAxes = new Axis[]
           {
                new Axis
                {
                    Name = "Insulin Levels",
                    NameTextSize = 50,
                    NamePaint = new SolidColorPaint(SKColors.Red),
                    MinStep = 1,
                    LabelsPaint = new SolidColorPaint(SKColors.Green),
                    TextSize = 72,
                }
           };
        }
        public LabelVisual Title { get; set; }
        public Axis[] XAxes { get; set; }

        public Axis[] YAxes { get; set; }
    }
}
