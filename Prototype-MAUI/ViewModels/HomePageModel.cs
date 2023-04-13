using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp8.Model;
using MauiApp8.Model2;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.BackgroundFetchService;
using MauiApp8.Services.BackgroundServices;
using MauiApp8.Services.GoogleFitService;
using MauiApp8.Services.PublishSubscribeService;
using MauiApp8.Services.ThirdPartyHealthService;
using Realms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using static Google.Apis.Requests.BatchRequest;


namespace MauiApp8.ViewModel
{
    public partial class HomePageModel : ObservableObject
    {
        IAuthenticationService authService;
        
        IBackgroundService _backgroundService;

        IBackgroundFetchService _backgroundFetchService;
        IThirdPartyHealthService _thirdPartyHealthService;
        Realm realm;

        

        Publish _publish;

        [ObservableProperty]
        string sgv;
        [ObservableProperty]
        string date;
        [ObservableProperty]
        string bgl;

        [ObservableProperty]
        MvvmHelpers.ObservableRangeCollection<GlucoseInfo> glucoseInfo;



        public HomePageModel(IAuthenticationService authService, IBackgroundService backgroundService, Realm _realm, IThirdPartyHealthService thirdPartyHealthService
, Publish publish, IBackgroundFetchService backgroundFetchService)
        {


            realm = _realm;
            _publish = publish;
            _backgroundService = backgroundService;
            _thirdPartyHealthService = thirdPartyHealthService;
            _backgroundFetchService = backgroundFetchService;
            Task.Run(() => InitializeAsync());
            

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
            
            ///Todo Make a the DateTime in sync with the bgService
            // For Tests
            DateTime now = DateTime.UtcNow;
            DateTime startTime = now.AddDays(-1);
            Console.WriteLine("-------------------------------------------------------");

            Console.WriteLine(now.ToString());

            Console.WriteLine(startTime.ToString());
            Console.WriteLine("-------------------------------------------------------");

            await _thirdPartyHealthService.FetchActivityDataAsync(now, startTime);
            await _thirdPartyHealthService.FetchCalorieDataAsync(now, startTime);

            foreach (var stepData in _thirdPartyHealthService.CommonHealthData.StepDataList)
            {
                Console.WriteLine($"Steps: {stepData.Steps}, Start Time: {stepData.StartTime}, End Time: {stepData.EndTime}");
            }

            foreach (var calorieData in _thirdPartyHealthService.CommonHealthData.CalorieDataList)
            {
                Console.WriteLine($"Calories: {calorieData.Calories}, Start Time: {calorieData.StartTime}, End Time: {calorieData.EndTime}");
            }

            foreach (var activityData in _thirdPartyHealthService.CommonHealthData.ActivityDataList)
            {
                Console.WriteLine($"count: {activityData.Count}, Start Time: {activityData.StartTime}, End Time: {activityData.EndTime}, Activity Duration: {activityData.ActivityDuration}, Activity Type: { activityData.ActivityType} ");
            }



            //await _publish.CheckSubscribe();
            Console.WriteLine("...");


        }
        [RelayCommand]
        async Task CallFunction() {

            Console.WriteLine("INN CALL");
            StartBackgroundFetch();
            //_backgroundFetchService.ScheduleFetchTask(TimeSpan.FromMinutes(1), () =>
            //{
            //    Console.WriteLine("BAckground.2");
            //});
            await _publish.UpdateBackgroundData("https://oskarnightscoutweb1.azurewebsites.net/");
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






        [ObservableProperty]
         Account user;

       


        public DateTimeOffset? ReadLatestGlucose()
        {
            return _backgroundService.ReadLatestGlucose();
        }


       

        

        
    }
}

