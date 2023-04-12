﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp8.Model;
using MauiApp8.Views;
using MauiApp8.Model2;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.BackgroundServices;
using Realms;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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

namespace MauiApp8.ViewModel
{
    public partial class HomePageModel : ObservableObject
    {
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
        MvvmHelpers.ObservableRangeCollection<GlucoseInfo> glucoseInfo;

        // Chart stuff
        private readonly IChartService _chartService;
        [ObservableProperty]
        private ISeries[] _series;
        public HomePageModel(IAuthenticationService authService, IBackgroundService backgroundService, Realm _realm, IChartService chartService)
        {


            realm = _realm;
            _backgroundService = backgroundService;
            Task.Run(() => InitializeAsync());

            var objects = realm.All<GlucoseInfo>(); // retrieve all objects of type MyObject from the database

            foreach (GlucoseInfo obj in objects)
            {
                Console.WriteLine($"Loaded {obj.Glucose} from db");
            }
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
            // your existing code here
          

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
            var amount = realm.All<InsulinInfo>().Count();
            Console.WriteLine($"{amount}");
            realm.Write(() =>
            {
                var dog = new InsulinInfo { Insulin = 13, Timestamp = new DateTimeOffset() };
                // Add the instance to the realm.
                realm.Add(dog);
            });
            var amount1 = realm.All<InsulinInfo>().Count();

            Console.WriteLine($"Loaded {amount1} from db");
        }

        
    }
}

