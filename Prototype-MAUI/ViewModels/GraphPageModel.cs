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
using LiveChartsCore.Defaults;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;

namespace MauiApp8.ViewModel
{
    public partial class GraphPageModel : ObservableObject
    {
        IChartService<HealthData> _chartService;
        IChartConfigurationProvider _chartConfigurationProvider;

        [ObservableProperty]
        ObservableCollection<ISeries> seriesChart;
        [ObservableProperty]
        ObservableCollection<ISeries> glucoseSeriesChart;
        [ObservableProperty]
        ObservableCollection<ISeries[]> testSeriesChart;
        [ObservableProperty]
        ISeries[] seriesBolosChart;
        [ObservableProperty]
        ChartConfiguration chartConfigurations;
        [ObservableProperty]
        public int lastInsulinLevel;
        [ObservableProperty]
        public int lastGlucoseLevel;
        [ObservableProperty]
        ChartConfiguration getBolosChartConfiguration;
        [ObservableProperty]
        DateTimeOffset fromDate;
        [ObservableProperty]
        DateTimeOffset toDate;

        private bool _isDataChanged = false;
        private int _dataChangedCounter = 0;

        [ObservableProperty]
        ObservableCollection<Model.GlucoseInfo> glucosesData;
        [ObservableProperty]
        ObservableCollection<Model.InsulinInfo> insulinsData;

        public GraphPageModel(IChartService<HealthData> chartService, IChartConfigurationProvider chartConfiguration)
        {
            _chartService = chartService;
            _chartConfigurationProvider = chartConfiguration;
            GlucoseSeriesChart = new ObservableCollection<ISeries>();
            SeriesChart = new ObservableCollection<ISeries>();
            getBolosChartConfiguration = new ChartConfiguration();
            chartConfigurations = new ChartConfiguration();





            Task.Run(() => InitializeAsync());
        }



        private async Task InitializeAsync()
        {

            Console.WriteLine("In init");
             FromDate = DateTimeOffset.UtcNow.AddDays(-1);
             ToDate = DateTimeOffset.UtcNow;
            _chartService.IsDataChanged = false;
            _chartService.DataChanged += async (sender, e) =>
            {
                if (!_chartService.IsDataChanged)
                {

                    _dataChangedCounter++;
                    SeriesChart.Clear();
                    GlucoseSeriesChart.Clear();
                    Console.WriteLine("Data changed");
                    var BolosSeries = await _chartService.AddBasalSeries();
                    var GlucoseSeries = await _chartService.AddGlucosesSeries();
                    GetBolosChartConfiguration = _chartConfigurationProvider.GetBolosChartConfiguration(FromDate, ToDate);
                    ChartConfigurations = _chartConfigurationProvider.GetChartConfiguration(FromDate, ToDate);
                    //SeriesChart.Add(BolosSeries);
                    //GlucoseSeriesChart.Add(GlucoseSeries);
                }
                Console.WriteLine(_dataChangedCounter);
            };


            _chartService.IsDataChanged = false;
            var basalSeries = await _chartService.AddBasalSeries();
            var glucoseSeries = await _chartService.AddGlucosesSeries();
            var InsulinSeries = await _chartService.AddInsulinSeries();

            SeriesChart.Add(basalSeries);
            GlucoseSeriesChart.Add(glucoseSeries);
            GlucoseSeriesChart.Add(InsulinSeries);

            this.GlucosesData = _chartService.GlucosesChart;
            this.InsulinsData = _chartService.InsulinsChart;
            LastGlucoseLevel = _chartService.LastPointInData.Glucose;
            LastInsulinLevel = _chartService.LastPointInData.Insulin;
           

            GetBolosChartConfiguration = _chartConfigurationProvider.GetBolosChartConfiguration(FromDate, ToDate);
            ChartConfigurations = _chartConfigurationProvider.GetChartConfiguration(FromDate, ToDate);

        }

    }
}