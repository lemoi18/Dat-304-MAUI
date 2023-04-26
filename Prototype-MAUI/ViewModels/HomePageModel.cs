using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Model;
using MauiApp8.Views;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.BackgroundFetchService;
using MauiApp8.Services.BackgroundServices;
using MauiApp8.Services.PublishSubscribeService;
using MauiApp8.Services.ThirdPartyHealthService;
using Realms;
using System.Collections.ObjectModel;
using LiveChartsCore;
using MauiApp8.Services.GraphService;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using MauiApp8.Services.Health;
using CommunityToolkit.Mvvm.Input;
using MauiApp8.Services.BackgroundServices.Realm;


namespace MauiApp8.ViewModel
{
    public partial class HomePageModel : ObservableObject
    {
        IAuthenticationService authService;
        IBackgroundService _backgroundService;
        IBackgroundFetchService _backgroundFetchService;
        IThirdPartyHealthService _thirdPartyHealthService;
        IChartConfigurationProvider _chartConfigurationProvider;
        IChartService<HealthData> _chartService;
        IHealthService _healthService;
        Publish _publish;
        ICRUD curd;

        [ObservableProperty]
         float lastGlucoseLevel;
        [ObservableProperty]

        float secondlastglcoseLevel;


        [ObservableProperty]
        ChartConfiguration chartConfigurations;
       
        [ObservableProperty]
        ChartConfiguration getBolosChartConfiguration;

        [ObservableProperty]
        DateTimeOffset fromDate;
        [ObservableProperty]
        DateTimeOffset toDate;

        [ObservableProperty]
        ObservableCollection<ISeries> seriesChart;
        [ObservableProperty]
        ObservableCollection<ISeries> glucoseSeriesChart;


        public HomePageModel(
            IAuthenticationService authService, 
            IBackgroundService backgroundService, 
            IChartService<HealthData> chartService, 
            IChartConfigurationProvider chartConfigurationProvider,
            Publish publish, 
            IBackgroundFetchService backgroundFetchService,
            IHealthService healthService
)
        {
            _chartConfigurationProvider = chartConfigurationProvider;
            _healthService = healthService;
            _publish = publish;
            _backgroundService = backgroundService;
            _backgroundFetchService = backgroundFetchService;
            _chartService = chartService;
           

            GlucoseSeriesChart = new ObservableCollection<ISeries>();
            SeriesChart = new ObservableCollection<ISeries>();
            getBolosChartConfiguration = new ChartConfiguration();
            chartConfigurations = new ChartConfiguration();

            Task.Run(()=> TestFunction());
            Task.Run(() => InitializeAsync());

        }
        
      

        private async Task InitializeAsync()
        {


            FromDate = DateTimeOffset.UtcNow.AddDays(-1);
            ToDate = DateTimeOffset.UtcNow;
            _chartService.IsDataChanged = false;
            _chartService.DataChanged += async (sender, e) =>
            {
                if (!_chartService.IsDataChanged)
                {

                    SeriesChart.Clear();
                    GlucoseSeriesChart.Clear();
                    var BolosSeries = await _chartService.AddBasalSeries();
                    var GlucoseSeries = await _chartService.AddGlucosesSeries();
                    GetBolosChartConfiguration = _chartConfigurationProvider.GetBolosChartConfiguration(FromDate, ToDate);
                    ChartConfigurations = _chartConfigurationProvider.GetChartConfiguration(FromDate, ToDate);
                    //SeriesChart.Add(BolosSeries);
                    //GlucoseSeriesChart.Add(GlucoseSeries);
                }
            };

            _chartService.IsDataChanged = false;
            var basalSeries = await _chartService.AddBasalSeries();
            var glucoseSeries = await _chartService.AddGlucosesSeries();
            var InsulinSeries = await _chartService.AddInsulinSeries();

            SeriesChart.Add(basalSeries);
            GlucoseSeriesChart.Add(glucoseSeries);
            GlucoseSeriesChart.Add(InsulinSeries);

            LastGlucoseLevel = _chartService.LastPointInData.LastGlucose;
            SecondlastglcoseLevel = _chartService.LastPointInData.SecondLastGlucose;
            
            GetBolosChartConfiguration = _chartConfigurationProvider.GetBolosChartConfiguration(FromDate, ToDate);
            ChartConfigurations = _chartConfigurationProvider.GetChartConfiguration(FromDate, ToDate);




        }

        [RelayCommand]
        async Task TestFunction()
        {
            //await _publish.CheckTimeDiffrence();
            //await _publish.GoogleFetchSub();
            await _publish.HealthSub();

        }



        [RelayCommand]
        async Task CallFunction()
        {
            await _publish.Turn_On();
        }

    }

            
}

  







      

        

        
  

