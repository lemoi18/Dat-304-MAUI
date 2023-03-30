using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CreateDBLib;
using MauiApp8.Model;
using MauiApp8.Services.Authentication;

namespace MauiApp8.ViewModel
{
    public partial class HomePageModel : ObservableObject
    {
        IAuthenticationService authService;
        private Realms.Realm _realm;


        public HomePageModel(IAuthenticationService authService, Realms.Realm db)
        {

            this._realm = db;
            this.authService = authService;
            _user = authService.User;
            // Access the CurrentUser property to get the user object

            Console.WriteLine($"Loaded {_realm} db from DBLib");
            var amount = db.All<CreateDBLib.InsulinInfo>().Count();
            Console.WriteLine($"{amount}");
            db.Write(() =>
            {
                var dog = new CreateDBLib.InsulinInfo { Insulin = 13, Timestamp = new DateTimeOffset() };
                // Add the instance to the realm.
                db.Add(dog);
            });
            var amount1 = db.All<InsulinInfo>().Count();

            Console.WriteLine($"Loaded {amount1} from db");

        }
        public Account User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }
        private Account _user;

        

        [RelayCommand]
        Task NavigateBack() => Shell.Current.GoToAsync("..");
       
    }
}

