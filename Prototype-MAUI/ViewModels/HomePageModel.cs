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
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Windows.Input;
using System;
using static Google.Apis.Requests.BatchRequest;
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

        Publish _publish;

        [ObservableProperty]
        string sgv;
        [ObservableProperty]
        string date;
        [ObservableProperty]
        string bgl;

        [ObservableProperty]
        MvvmHelpers.ObservableRangeCollection<MauiApp8.Model.GlucoseInfo> glucoseInfo;
        private readonly IChartService<HealthData> _chartService;
        IHealthService _healthService;
        [ObservableProperty]
         ISeries[] series;

        private readonly ICRUD curd;

        [ObservableProperty]

        ObservableCollection<Model.GlucoseInfo> glucoses;
        [ObservableProperty]

        ObservableCollection<Model.InsulinInfo> insulins;



        [ObservableProperty]
        ChartConfiguration chartConfiguration;


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
            Task.Run(() => InitializeAsync());

            _chartService = chartService;

            DateTimeOffset fromDate = DateTimeOffset.UtcNow.AddDays(-1);
            DateTimeOffset toDate = DateTimeOffset.UtcNow;

            Glucoses = new ObservableCollection<Model.GlucoseInfo>();
            Insulins = new ObservableCollection<Model.InsulinInfo>();



            _publish.GlucoseDataAvailable += (sender, e) =>
            {
                foreach (var glucose in e.GlucoseData)
                {
                    Glucoses.Add(glucose);
                }
                LastGlucoseLevel = int.TryParse(Glucoses.Where(g => g.Timestamp <= toDate)
                .LastOrDefault()?.Glucose.ToString() ?? "0", out int glucoseLevel) ? glucoseLevel : 0;
            };

            _publish.InsulinDataAvailable += (sender, e) =>
            {
                foreach (var insulin in e.InsulinData)
                {
                    Insulins.Add(insulin);
                }
                LastInsulinLevel = int.TryParse(Insulins.Where(g => g.Timestamp <= toDate)
                               .LastOrDefault()?.Insulin.ToString() ?? "0", out int insulinLevel) ? insulinLevel : 0;
            };


            // extract last value from the LineSeries and assign to public property
            
        }
        [ObservableProperty]
        public int lastInsulinLevel;
        [ObservableProperty]
        public int lastGlucoseLevel;
      

        private async Task InitializeAsync()
        {

           

            await TestFunction();
            await GetHealthData();
            this.Series = await _chartService.GetSeries();

            var glucoseTimestampStrings = _chartService.GlucosesChart.Select(g => g.Timestamp.ToString("HH:mm")).ToArray();

            ChartConfiguration = _chartConfigurationProvider.GetChartConfiguration(glucoseTimestampStrings);
            LastGlucoseLevel = _chartService.LastPointInData.Glucose;
            LastInsulinLevel = _chartService.LastPointInData.Insulin;
        }

        [RelayCommand]
        async Task TestFunction()
        {
            await _publish.CheckTimeDiffrence();
            await _publish.GoogleFetchSub();
            await _publish.HealthSub();

        }




        [RelayCommand]
        async Task GetHealthData()
        {
            DateTimeOffset toDate = DateTimeOffset.UtcNow;
            DateTimeOffset fromDate = toDate.AddDays(-1);
            

            var glucose = await _healthService.ReadGlucoses(
                fromDate,
                toDate);
            var insulin = await _healthService.ReadInsulins(fromDate, toDate);
            foreach (var item in insulin)
            {
                Insulins.Add(item);
                Console.WriteLine(item);

            }
            foreach (var item in glucose)
            {
                Glucoses.Add(item);
                

            }
            //LastGlucoseLevel = int.TryParse(Glucoses.LastOrDefault()?.Glucose.ToString() ?? "0", out int glucoseLevel) ? glucoseLevel : 0;
            LastGlucoseLevel = int.TryParse(Glucoses.Where(g => g.Timestamp <= toDate)
            .LastOrDefault()?.Glucose.ToString() ?? "0", out int glucoseLevel) ? glucoseLevel : 0;

            LastInsulinLevel = int.TryParse(Insulins.Where(g => g.Timestamp <= toDate)
                               .LastOrDefault()?.Insulin.ToString() ?? "0", out int insulinLevel) ? insulinLevel : 0;

        }
        [RelayCommand]
        async Task CallFunction()
        {
            await _publish.Turn_On();
        }

    }

            
}

  







      

        

        
  

