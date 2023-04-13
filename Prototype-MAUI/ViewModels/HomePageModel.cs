using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Model;
using MauiApp8.Views;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.BackgroundServices;
using MauiApp8.Services.BackgroundServices.Realm;
using Realms;
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
        private readonly ICRUD _crudStub;


        public ICommand RunTestCommand => new Command(RunCRUDTest);

        private void RunCRUDTest()
        {
            _crudStub.Test();
        }

        IAuthenticationService authService;

        IBackgroundService _backgroundService;

        Realm realm;


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

        public HomePageModel(IAuthenticationService authService, IBackgroundService backgroundService, Realm _realm, ICRUD crudStub, IChartService chartService, IHealthService healthService)
        {
            _crudStub = crudStub;
            _healthService = healthService;

            // Find out how to read glucose & Insulin data, Fix healthService injection

            realm = _realm;
            _backgroundService = backgroundService;
            Task.Run(() => InitializeAsync());

            var objects = realm.All<Services.BackgroundServices.Realm.GlucoseInfo>();


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
            await UpdateStuff();
            //NavigateToFoodDetailsCommand = new RelayCommand<FoodViewModel>(NavigateToFoodDetails);
            //await UpdateStuff();
        }

        private async Task UpdateStuff()
        {
            await _backgroundService.UpdateGlucose("https://oskarnightscoutweb1.azurewebsites.net/");
            await _backgroundService.UpdateInsulin("https://oskarnightscoutweb1.azurewebsites.net/");
        }

        public static async Task<List<GlucoseAPI>> GetGlucose(string RestUrl, string StartDate, string EndDate)
        {
            JsonSerializerOptions _serializerOptions;

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            Console.WriteLine($"Starting GetGlucose method with RestUrl={RestUrl}, StartDate={StartDate}, EndDate={EndDate}...");

            List<GlucoseAPI> Items;
            Items = new List<GlucoseAPI>();
            string Order = $"api/v1/entries/sgv.json?find[dateString][$gte]={StartDate}&find[dateString][$lte]={EndDate}&count=all";
            Uri uri = new Uri($"{RestUrl}{Order}");

            try
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

                    response.EnsureSuccessStatusCode(); 
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetGlucose method: {ex.Message}");
                Debug.WriteLine($"Error in GetGlucose method: {ex}");
                Console.WriteLine($"URI: {uri}");
               
                return Items;
            }

            Console.WriteLine("Finished GetGlucose method.");
            return Items;
        }

        public Account User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        private Account _user;

        public DateTimeOffset? ReadLatestGlucose()
        {
            return _backgroundService.ReadLatestGlucose();
        }

        public void DBTest()
        {
            Console.WriteLine($"Loaded {realm} db from DBLib");
            var amount = realm.All<Services.BackgroundServices.Realm.InsulinInfo>().Count();
            Console.WriteLine($"{amount}");
            realm.Write(() =>
            {
                var dog = new Services.BackgroundServices.Realm.InsulinInfo { Insulin = 13, Timestamp = new DateTimeOffset() };
                
                realm.Add(dog);
            });
            var amount1 = realm.All<Services.BackgroundServices.Realm.InsulinInfo>().Count();

            Console.WriteLine($"Loaded {amount1} from db");
        }
    }
}

