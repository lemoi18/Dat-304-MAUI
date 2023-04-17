using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MauiApp8.Services.GraphService;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using MauiApp8.Services.Health;
using MauiApp8.ViewModel;
using MauiApp8.Model;
using System.Collections.ObjectModel;

namespace MauiApp8.ViewModel
{
    public partial class GraphPageModel : ObservableObject
    {
        IChartService<HealthData> _chartService;
        IChartConfigurationProvider _chartConfigurationProvider;
        public GraphPageModel(IChartService<HealthData> chartService, IChartConfigurationProvider chartConfiguration)
        {
            _chartService = chartService;
            _chartConfigurationProvider = chartConfiguration;
            Task.Run(()=>InitializeAsync());
        }

        [ObservableProperty]
        ISeries[] seriesChart;
        [ObservableProperty]
        ChartConfiguration chartConfiguration;
        [ObservableProperty]
        public int lastInsulinLevel;
        [ObservableProperty]
        public int lastGlucoseLevel;

        public SolidColorPaint LegendTextPaint { get; set; }
        public SolidColorPaint LegendBackgroundPaint { get; set; }
        public LabelVisual Title { get; set; }
        public Axis[] XAxes { get; set; }

        public Axis[] YAxes { get; set; }

        [ObservableProperty]
        ObservableCollection<Model.GlucoseInfo> glucosesChart;
        [ObservableProperty]
        ObservableCollection<Model.InsulinInfo> insulinsChart;
        

        private async Task InitializeAsync()
        {
            this.GlucosesChart = _chartService.GlucosesChart;
            this.InsulinsChart = _chartService.InsulinsChart;
            LastGlucoseLevel = _chartService.LastPointInData.Glucose;
            LastInsulinLevel = _chartService.LastPointInData.Insulin;
            SeriesChart = await _chartService.GetSeries();

            DateTimeOffset fromDate = DateTimeOffset.UtcNow.AddDays(-1);
            DateTimeOffset toDate = DateTimeOffset.UtcNow;
            var timestapts = GlucosesChart.Select(g => g.Timestamp.ToString("HH:mm")).ToArray();
            ChartConfiguration = _chartConfigurationProvider.GetChartConfiguration(timestapts);

            // Implement chart-specific configuration
        }
    }

    

    
}
