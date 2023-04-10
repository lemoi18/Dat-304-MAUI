using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MauiApp8.Services.GraphService;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;

namespace MauiApp8.ViewModel
{
    public partial class GraphPageModel : ObservableObject
    {
        private readonly IChartService _chartService;
        [ObservableProperty]
        private ISeries[] _series;

        public GraphPageModel(IChartService chartService)
        {
            _chartService = chartService;
            _series = _chartService.GetSeries();
            Title = new LabelVisual
            {
                Text = "Insulin Health Data",
                TextSize = 25,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };
        }
        public LabelVisual Title { get; set; }

    }
}
