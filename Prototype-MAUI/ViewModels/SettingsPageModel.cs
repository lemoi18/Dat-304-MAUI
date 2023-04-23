using CommunityToolkit.Mvvm.Input;
using MauiApp8.Views;
using MauiApp8.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.Health;

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


        [RelayCommand]
        async Task<string> DeleteData()
        {

            DateTime now = DateTime.UtcNow;
            DateTime startTime = now.AddDays(-30);
            await _healthService.DeleteGlucoseData(startTime, now);
            await _healthService.DeleteInsulinData(startTime, now);

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
