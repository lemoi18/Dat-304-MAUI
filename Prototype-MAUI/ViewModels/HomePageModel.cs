using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp8.Model;
using MauiApp8.Model2;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.BackgroundServices;
using MauiApp8.Services.GoogleFitService;
using MauiApp8.Services.PublishSubscribeService;
using Realms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using static Google.Apis.Requests.BatchRequest;
using Microsoft.Maui.Controls;

namespace MauiApp8.ViewModel
{
    public partial class HomePageModel : ObservableObject
    {
        IAuthenticationService authService;
        
        IBackgroundService _backgroundService;

        Services.BackgroundFetchService.IBackgroundFetchService _backgroundFetchService;

        Realm realm;

        GoogleFit _googlefit;

        Publish _publish;

        [ObservableProperty]
        string sgv;
        [ObservableProperty]
        string date;
        [ObservableProperty]
        string bgl;

        [ObservableProperty]
        MvvmHelpers.ObservableRangeCollection<GlucoseInfo> glucoseInfo;



        public HomePageModel(IAuthenticationService authService, IBackgroundService backgroundService, Realm _realm, GoogleFit googlefit, Publish publish, Services.BackgroundFetchService.IBackgroundFetchService backgroundFetchService)
        {


            realm = _realm;
            _publish = publish;
            _backgroundService = backgroundService;
            _googlefit = googlefit;
            _backgroundFetchService = backgroundFetchService;
            Task.Run(() => InitializeAsync());
            
            var objects = realm.All<GlucoseInfo>(); // retrieve all objects of type MyObject from the database

            //foreach (GlucoseInfo obj in objects)
            //{
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
            //await UpdateStuff();
            //NavigateToFoodDetailsCommand = new RelayCommand<FoodViewModel>(NavigateToFoodDetails);
            //await UpdateStuff();
        }

        [RelayCommand]
        async Task TestFunction()
        {
            //_googlefit.CheckAccount();
            string iso8601DateTime = "2023-04-01T15:53:17";
            long unixTimeMilliseconds = _googlefit.ConvertToUnixTimeMilliseconds(iso8601DateTime);
            int currentUnixTimestamp = _googlefit.GetCurrentUnixTimestamp();
            string currentTime = currentUnixTimestamp.ToString() + "000";
            string startTime = unixTimeMilliseconds.ToString();
            Console.WriteLine(currentTime);
            Console.WriteLine(startTime);
            await _publish.CheckSubscribe();
            Console.WriteLine("...");


        }
        [RelayCommand]
        async Task CallFunction() {

            Console.WriteLine("INN CALL");
            //_backgroundFetchService.ScheduleFetchTask(TimeSpan.FromMinutes(1), () =>
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
#if __ANDROID__
            //IBackgroundService backgroundFetchService = new MauiApp8.Platforms.Android.AndroidServices.BackgroundFetchServiceAndroid();

            // Define your fetch action
            Action fetchAction = () =>
            {
                // Your background fetch logic here
                // Logic in Background reciver in android directory
            };

            // Call the ScheduleFetchTask method to schedule the background fetch every 1 minute
            _backgroundFetchService.ScheduleFetchTask(TimeSpan.FromMinutes(1), fetchAction);
#endif
        }


        private async Task UpdateStuff()
        {

            
                await _backgroundService.UpdateGlucose("https://oskarnightscoutweb1.azurewebsites.net/");
                await _backgroundService.UpdateInsulin("https://oskarnightscoutweb1.azurewebsites.net/");
            


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

