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


        [ObservableProperty]

        IAuthenticationService _authService;
        private CancellationTokenSource _cancellationTokenSource;

        [ObservableProperty]
        Account user;
        public LoginPageModel(IAuthenticationService authService)
        {

            AuthService = authService;


        }



        [RelayCommand]
        Task NavigateToHome() => Shell.Current.GoToAsync(nameof(HomePage));



      

        [RelayCommand]
        async Task NavigateToGoogle()
        {

            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                User = await AuthService.AuthenticateAsync(_cancellationTokenSource.Token);
            _cancellationTokenSource.Token.ThrowIfCancellationRequested();

            if (User.LoginSuccessful)
            {
                Application.Current.MainPage = new AppShell();
            }
            }
            catch (OperationCanceledException)
            {

                await Shell.Current.DisplayAlert(
                         "Error",
                          $"An error occurred duing authentication, please try again",
                          "Close");


            }


        }

        

    }

}
