using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp8.Model;
using MauiApp8.Model2;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.BackgroundServices;
using Realms;

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


        public HomePageModel(IAuthenticationService authService, IBackgroundService backgroundService, Realm _realm)
        {


            realm = _realm;
            _backgroundService = backgroundService;
            InitializeAsync();
            Console.WriteLine($"Loaded {realm} db from viewmodel");
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

        private async Task InitializeAsync()
        {
            await Task.Run(() => UpdateStuff());
            //NavigateToFoodDetailsCommand = new RelayCommand<FoodViewModel>(NavigateToFoodDetails);
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

