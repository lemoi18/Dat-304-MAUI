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

        Publish _publish;

        [ObservableProperty]
        string sgv;
        [ObservableProperty]
        string date;
        [ObservableProperty]
        string bgl;

        [ObservableProperty]
        MvvmHelpers.ObservableRangeCollection<MauiApp8.Model.GlucoseInfo> glucoseInfo;
        private readonly IChartService _chartService;
        IHealthService _healthService;
        [ObservableProperty]
        private ISeries[] _series;

        private readonly ICRUD curd;

        [ObservableProperty]

        ObservableCollection<Model.GlucoseInfo> glucoses;
        [ObservableProperty]

        ObservableCollection<Model.InsulinInfo> insulins;

        public HomePageModel(IAuthenticationService authService, IBackgroundService backgroundService, IChartService chartService, IHealthService healthService, Publish publish, IBackgroundFetchService backgroundFetchService)
        {
            _healthService = healthService;

            
            _publish = publish;
            _backgroundService = backgroundService;
            _backgroundFetchService = backgroundFetchService;
            Task.Run(() => InitializeAsync());

            _chartService = chartService;
            _series = _chartService.GetSeries();

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
            LastInsulinLevel = ((LineSeries<int>)_series[0]).Values.LastOrDefault();
            LastGlucoseLevel = ((LineSeries<int>)_series[1]).Values.LastOrDefault();

            Title = new LabelVisual
            {
                Text = "Insulin Levels",
                TextSize = 25,
                Padding = new LiveChartsCore.Drawing.Padding(15),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };
        }
        public LabelVisual Title { get; set; }
        [ObservableProperty]
        public int lastInsulinLevel;
        [ObservableProperty]
        public int lastGlucoseLevel;
        public Axis[] XAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    Name = "X Axis",
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    MinStep = 1,

                    LabelsPaint = new SolidColorPaint(SKColors.Blue),
                    TextSize = 10,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 2 }
                }
            };
        
        public Axis[] YAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    Name = "Y Axis",
                    NamePaint = new SolidColorPaint(SKColors.Red),
                    MinStep = 1,
                    LabelsPaint = new SolidColorPaint(SKColors.Green),
                    TextSize = 20,

                    SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
                    {
                        StrokeThickness = 2,
                        PathEffect = new DashEffect(new float[] { 3, 3 })
                    }
                }
            };
        private async Task InitializeAsync()
        {

           

            await TestFunction();
            await Task.Run(()=> GetHealthData());
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
            DateTimeOffset fromDate = DateTimeOffset.UtcNow.AddDays(-1);
            DateTimeOffset toDate = DateTimeOffset.UtcNow;

            var glucose = await _healthService.ReadGlucoses(
                fromDate,
                toDate);
            var insulin = await _healthService.ReadInsulins(fromDate, toDate);
            foreach (var item in insulin)
            {
                Insulins.Add(item);

            }
            foreach (var item in glucose)
            {
                Glucoses.Add(item);
                

            }
            //LastGlucoseLevel = int.TryParse(Glucoses.LastOrDefault()?.Glucose.ToString() ?? "0", out int glucoseLevel) ? glucoseLevel : 0;
            LastInsulinLevel = int.TryParse(Insulins.LastOrDefault()?.Insulin.ToString() ?? "0", out int insulinLevel) ? insulinLevel : 0;
            LastGlucoseLevel = int.TryParse(Glucoses.Where(g => g.Timestamp <= toDate)
            .LastOrDefault()?.Glucose.ToString() ?? "0", out int glucoseLevel) ? glucoseLevel : 0;

        }
        [RelayCommand]
        async Task CallFunction()
        {
            await _publish.Turn_On();
        }

    }

            
}

  







      

        

        
  

