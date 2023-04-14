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
            InitializeAsync();
        }

        [ObservableProperty]
        ISeries[] seriesChart;
        [ObservableProperty]
        ChartConfiguration chartConfiguration;
        public SolidColorPaint LegendTextPaint { get; set; }
        public SolidColorPaint LegendBackgroundPaint { get; set; }
        public LabelVisual Title { get; set; }
        public Axis[] XAxes { get; set; }

        public Axis[] YAxes { get; set; }


        ObservableCollection<Model.GlucoseInfo> glucosesChart => _chartService.GlucosesChart;
        ObservableCollection<Model.InsulinInfo> insulinsChart => _chartService.InsulinsChart;
        

        private async Task InitializeAsync()
        {
            SeriesChart = await _chartService.GetSeries();

            DateTimeOffset fromDate = DateTimeOffset.UtcNow.AddDays(-1);
            DateTimeOffset toDate = DateTimeOffset.UtcNow;
            var timestapts = glucosesChart.Select(g => g.Timestamp.ToString("HH:mm")).ToArray();
            ChartConfiguration = _chartConfigurationProvider.GetChartConfiguration(timestapts);

            // Implement chart-specific configuration
        }
    }

    

    
}
