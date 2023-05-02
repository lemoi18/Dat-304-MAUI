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
using MauiApp8.Services.PublishSubscribeService;

namespace MauiApp8.ViewModel
{
    public partial class GraphPageModel : ObservableObject, IRecipient<GlucoseChartMessage>, IRecipient<InsulinChartMessage>
    {
        IChartService<HealthData> _chartService;
        IChartConfigurationProvider _chartConfigurationProvider;

        [ObservableProperty]
        ObservableCollection<ISeries> seriesChart;
        [ObservableProperty]
        ObservableCollection<ISeries> glucoseSeriesChart;
        
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
        ObservableCollection<GlucoseInfo> glucosesData;
        [ObservableProperty]
        ObservableCollection<Model.InsulinInfo> insulinsData;   

        public GraphPageModel(IChartService<HealthData> chartService, IChartConfigurationProvider chartConfiguration)
        {
            _chartService = chartService;
            WeakReferenceMessenger.Default.Send(new Fetch.Update_Health { Response = 101 });

            _chartConfigurationProvider = chartConfiguration;
            GlucoseSeriesChart = new ObservableCollection<ISeries>();
            SeriesChart = new ObservableCollection<ISeries>();
            getBolosChartConfiguration = new ChartConfiguration();
            chartConfigurations = new ChartConfiguration();

            WeakReferenceMessenger.Default.Register<GlucoseChartMessage>(this);
            WeakReferenceMessenger.Default.Register<InsulinChartMessage>(this);


            Task.Run(() => InitializeAsync());
        }



        private async Task InitializeAsync()
        {

            Console.WriteLine("In init");
             FromDate = DateTimeOffset.UtcNow.AddDays(-1);
             ToDate = DateTimeOffset.UtcNow;
            
            var basalSeries = await _chartService.AddBasalSeries();
            var glucoseSeries = await _chartService.AddGlucosesSeries();
            var InsulinSeries = await _chartService.AddInsulinSeries();

            SeriesChart.Add(basalSeries);
            GlucoseSeriesChart.Add(glucoseSeries);
            GlucoseSeriesChart.Add(InsulinSeries);

            this.GlucosesData = _chartService.GlucosesChart;
            this.InsulinsData = _chartService.InsulinsChart;
           

            GetBolosChartConfiguration = _chartConfigurationProvider.GetBolosChartConfiguration();
            ChartConfigurations = _chartConfigurationProvider.GetChartConfiguration();

        }

        [RelayCommand]
        void AddInsulin(ISeries insulin)
        {

            SeriesChart.Clear();
            SeriesChart.Add(insulin);
            //GetBolosChartConfiguration = _chartConfigurationProvider.GetBolosChartConfiguration();


        }


        [RelayCommand]
        void AddGlucose(ISeries glucose)
        {

            GlucoseSeriesChart.Clear();
            GlucoseSeriesChart.Add(glucose);
            //ChartConfigurations = _chartConfigurationProvider.GetChartConfiguration();


        }
        public void Receive(InsulinChartMessage message)
        {
            Console.WriteLine("Received InsulinChartMessage");

            MainThread.BeginInvokeOnMainThread(() =>
            {
                AddInsulin(message.Value);

            });
        }

        public void Receive(GlucoseChartMessage message)
        {
            Console.WriteLine("Received GlucoseChartMessage");

            MainThread.BeginInvokeOnMainThread(() =>
                {
                    AddGlucose(message.Value);

                });

         
        }
    }
}