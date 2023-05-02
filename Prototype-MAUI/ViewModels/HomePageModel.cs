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
using CommunityToolkit.Mvvm.Messaging;

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
        DateTimeOffset fromDate;
        [ObservableProperty]
        DateTimeOffset toDate;

        
        [ObservableProperty]
        ObservableCollection<ISeries> glucoseSeriesChartHome;


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

            GlucoseSeriesChartHome = new ObservableCollection<ISeries>();
            chartConfigurations = new ChartConfiguration();

            Task.Run(()=> TestFunction());
            Task.Run(() => InitializeAsync());

        }
        
      

        private async Task InitializeAsync()
        {


            FromDate = DateTimeOffset.UtcNow.AddDays(-1);
            ToDate = DateTimeOffset.UtcNow;
           
            LastGlucoseLevel = _chartService.LastPointInData.LastGlucose;
            SecondlastglcoseLevel = _chartService.LastPointInData.SecondLastGlucose;
           
            ChartConfigurations = _chartConfigurationProvider.GetChartConfiguration();




        }

        [RelayCommand]
        async Task TestFunction()
        {
            //await _publish.CheckTimeDiffrence();
            //await _publish.GoogleFetchSub();
            await _publish.HealthSub();
            WeakReferenceMessenger.Default.Send(new Fetch.Update_Health { Response = 101 });


        }



        [RelayCommand]
        async Task CallFunction()
        {
            await _publish.Turn_On();
        }


        [RelayCommand]
        void AddInsulin(ISeries insulin)
        {

            GlucoseSeriesChartHome.Clear();
            GlucoseSeriesChartHome.Add(insulin);

        }


        [RelayCommand]
        void AddGlucose(ISeries glucose)
        {

            GlucoseSeriesChartHome.Clear();
            GlucoseSeriesChartHome.Add(glucose);
            ChartConfigurations = _chartConfigurationProvider.GetChartConfiguration();


        }
        public void Receive(InsulinChartMessage message)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                AddInsulin(message.Value);

            });
        }

        public void Receive(GlucoseChartMessage message)
        {

            MainThread.BeginInvokeOnMainThread(() =>
            {
                AddGlucose(message.Value);

            });


        }
    }

            
}

  







      

        

        
  

