using CommunityToolkit.Mvvm.Input;
using MauiApp8.Views;
using MauiApp8.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.Health;
using MauiApp8.Services.BackgroundServices;
using System.Diagnostics;

namespace MauiApp8.ViewModel
{
    public partial class SettingsPageModel : ObservableObject
    {

        IAuthenticationService authService;
        IHealthService _healthService;

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
        private long _elapsedTime;
        public long ElapsedTime
        {
            get { return _elapsedTime; }
            set { SetProperty(ref _elapsedTime, value); }
        }

        [RelayCommand]
        async Task TestDBInput()
        {
            DataBase DB = new DataBase();
            Console.WriteLine("-----------------------------------------------------------------------------------------------------");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await DB.TestDBAmountInput(NumberToAdd);
            stopwatch.Stop();
            Console.WriteLine($"Amount of data added: {NumberToAdd}");
            Console.WriteLine($"Stopwatch time elapsed: {stopwatch.ElapsedMilliseconds} ms");
            ElapsedTime = stopwatch.ElapsedMilliseconds;
        }

        [RelayCommand]
        async Task<string> ConsoleLog()
        {
            Console.WriteLine("Button press registered!");
            return "Data added successfully";
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
