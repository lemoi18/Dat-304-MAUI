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




        [RelayCommand]
        async Task<Page> SignOut()
        {
            await authService.SignOutAsync();
            return Application.Current.MainPage = new LoginShell();
        }
    }
}
