using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Model;
using MauiApp8.Model2;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.BackgroundServices;
using MauiApp8.Services.BackgroundServices.Realm;
using Realms;
using System.Diagnostics;
using System.Text.Json;
using System.Windows.Input;

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
        MvvmHelpers.ObservableRangeCollection<MauiApp8.Model2.GlucoseInfo> glucoseInfo;



        public HomePageModel(IAuthenticationService authService, IBackgroundService backgroundService, Realm _realm, ICRUD crudStub)
        {
            _crudStub = crudStub;

            realm = _realm;
            _backgroundService = backgroundService;
            Task.Run(() => InitializeAsync());

            //var objects = realm.All<Services.BackgroundServices.Realm.GlucoseInfo>(); // retrieve all objects of type MyObject from the database

            //foreach (Services.BackgroundServices.Realm.GlucoseInfo obj in objects)
            //{
            //    Console.WriteLine($"Loaded {obj.Glucose} from db");
            //}
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
            var amount = realm.All<Services.BackgroundServices.Realm.InsulinInfo>().Count();
            Console.WriteLine($"{amount}");
            realm.Write(() =>
            {
                var dog = new Services.BackgroundServices.Realm.InsulinInfo { Insulin = 13, Timestamp = new DateTimeOffset() };
                // Add the instance to the realm.
                realm.Add(dog);
            });
            var amount1 = realm.All<Services.BackgroundServices.Realm.InsulinInfo>().Count();

            Console.WriteLine($"Loaded {amount1} from db");
        }


    }
}

