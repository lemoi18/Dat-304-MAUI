using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp8.Model;
using MauiApp8.Views;
using MauiApp8.Helpers;
using Microsoft.Maui.Networking;
using MauiApp8.Services.Authentication;

namespace MauiApp8.ViewModel
{

    public partial class LoginPageModel : ObservableObject
    {



        IAuthenticationService _authService;
        private Account _user;


        public LoginPageModel(IAuthenticationService authService)
        {

            this._authService = authService;


        }

        public Account User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }


       
        



        [RelayCommand]
        Task NavigateToSettings() => Shell.Current.GoToAsync(nameof(SettingsPage));
        [RelayCommand]
        Task NavigateToHome() => Shell.Current.GoToAsync(nameof(HomePage));


        [RelayCommand]
        async Task NavigateToGoogle()
        {
            User = await _authService.AuthenticateAsync();
            _authService.User = User;
            if (User.LoginSuccessful)
            {
                Application.Current.MainPage = new AppShell();
            }
            
        }

        

    }

}
