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
        // Chart stuff
        private readonly IChartService _chartService;
        private readonly IHealthService _healthService;
        [ObservableProperty]
        private ISeries[] _series;



        public HomePageModel(IAuthenticationService authService, IBackgroundService backgroundService, IChartService chartService, IHealthService healthService)
        {
            _healthService = healthService;


            _publish = publish;
            _backgroundService = backgroundService;
            _backgroundFetchService = backgroundFetchService;
            Task.Run(() => InitializeAsync());



            // chart stuff
            _chartService = chartService;
            _series = _chartService.GetSeries();

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
        public int LastInsulinLevel { get; set; }
        public int LastGlucoseLevel { get; set; }
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

            
            //    Console.WriteLine($"Loaded {obj.Glucose} from db");
            //}
            //_backgroundFetchService = backgroundFetchService; // Get the service using dependency injection
            //_backgroundFetchService.ScheduleFetchTask(TimeSpan.FromMinutes(2), () =>
            //{
            //    Console.WriteLine("BAckground.2");
            //});
           
        }
        
        private async Task InitializeAsync()
        {
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

        public Command PrintDataCommand => new Command(async () => await ExecutePrintDataCommand());

        private async Task ExecutePrintDataCommand()
        {
            try
            {
                var glucoses = _healthService.ReadGlucoses(DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
                var insulins = _healthService.ReadInsulins(DateTimeOffset.MinValue, DateTimeOffset.MaxValue);

                Debug.WriteLine("Glucoses:");
                foreach (var glucose in glucoses)
                {
                    Debug.WriteLine($"Glucose level: {glucose.Glucose}, Date: {glucose.Timestamp}");
                }

                Debug.WriteLine("Insulins:");
                foreach (var insulin in insulins)
                {
                    Debug.WriteLine($"Insulin dose: {insulin.Insulin}, Date: {insulin.Timestamp}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }
        }
        private async Task InitializeAsync()
        {
            await TestFunction();
            //await UpdateStuff();
            await UpdateStuff();
        }


        private async Task UpdateStuff()
        {
            
            
            // For Tests
           



            //await _publish.CheckSubscribe();
            Console.WriteLine("...");
            await _publish.CheckTimeDiffrence();
            await _publish.GoogleFetchSub();

        }
        [RelayCommand]
        async Task CallFunction() {


        public static async Task<List<GlucoseAPI>> GetGlucose(string RestUrl, string StartDate, string EndDate)
            //{
            //    Console.WriteLine("BAckground.2");
            //});
            
            await _publish.Turn_On();
            //await _publish.UpdateBackgroundData("https://oskarnightscoutweb1.azurewebsites.net/");
            Console.WriteLine("OUT CALL");
            
        }

        public void StartBackgroundFetch()
        {
            // Create an instance of BackgroundFetchServiceAndroid
            _serializerOptions = new JsonSerializerOptions
            //IBackgroundService backgroundFetchService = new MauiApp8.Platforms.Android.AndroidServices.BackgroundFetchServiceAndroid();

            // Define your fetch action
            Action fetchAction = () =>
            {
                // Your background fetch logic here
            _backgroundFetchService.ScheduleFetchTask(TimeSpan.FromMinutes(1), fetchAction);
#endif
        }


            {
                using (HttpClient _client = new HttpClient())
                {
                    Console.WriteLine($"Getting Response... from {uri}");
                    HttpResponseMessage response = await _client.GetAsync(uri);
                    if (response.Content.Headers.ContentLength == 0)
                    {
                        Debug.WriteLine("The response content is empty.");
                    }
                    else if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        Items = JsonSerializer.Deserialize<List<GlucoseAPI>>(content, _serializerOptions);
                        Console.WriteLine("Finnished request...");
                        Console.WriteLine($"Received {Items.Count} items from API: {string.Join(", ", Items.Select(i => i.ToString()))}");

                    }
                    else
                    {
                        Console.WriteLine($"Error in GetGlucose method: Received status code {response.StatusCode}");
                        throw new HttpRequestException($"Error in GetGlucose method: Received status code {response.StatusCode}");
                    }

                    response.EnsureSuccessStatusCode(); // This will throw an exception if the status code is not a success code (2xx)
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetGlucose method: {ex.Message}");
                Debug.WriteLine($"Error in GetGlucose method: {ex}");
                Console.WriteLine($"URI: {uri}");
                // you could also display an error message to the user here, or retry the request if appropriate
                return Items;
            }

        }






      

        

        
    }
}

            Console.WriteLine($"Loaded {amount1} from db");
        }

        
        
    }
}

