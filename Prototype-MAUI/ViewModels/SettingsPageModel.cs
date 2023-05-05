using CommunityToolkit.Mvvm.Input;
using MauiApp8.Views;
using MauiApp8.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.Health;
using MauiApp8.Services.BackgroundServices;

namespace MauiApp8.ViewModel
{
    public partial class SettingsPageModel : ObservableObject
    {

        IAuthenticationService authService;
        IHealthService _healthService;
        private Account _user;
        public Account User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }


        public SettingsPageModel(IAuthenticationService _authService, IHealthService healthService) 
        {
            this.authService = _authService;
            _healthService = healthService;
        }

        [RelayCommand]
        Task NavigateBack() => Shell.Current.GoToAsync("..");


        private int _numberToAdd;

        public int NumberToAdd
        {
            get => _numberToAdd;
            set => SetProperty(ref _numberToAdd, value);
        }

        [RelayCommand]
        async Task<string> TestDBInput()
        {
            DataBase DB = new DataBase();
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            DB.TestDBAmountInput(NumberToAdd);
            Console.WriteLine($"Amount of data added: {NumberToAdd}");
            return "Data deleted successfully";
        }

        [RelayCommand]
        async Task<string> TestDBDelete()
        {
            DataBase DB = new DataBase();
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            DB.TestDBAmountDEL();

            return "Data deleted successfully";
        }



        [RelayCommand]
        async Task<Page> SignOut()
        {
            await authService.SignOutAsync();
            return Application.Current.MainPage = new LoginShell();
        }
    }
}
