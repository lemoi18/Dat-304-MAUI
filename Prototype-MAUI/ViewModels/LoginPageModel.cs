using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp8.Model;
using MauiApp8.Views;
using MauiApp8.Helpers;
using Microsoft.Maui.Networking;
using MauiApp8.Services.Authentication;
using CommunityToolkit.Mvvm.Messaging;

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
                    Console.WriteLine("before Send ");
                    Console.WriteLine(User.LoginSuccessful);


                    WeakReferenceMessenger.Default.Send(new Model.Alarms.Authenticate { isAuth = User.LoginSuccessful });

                }
                else
                {
                    await Shell.Current.DisplayAlert(
                        "Error",
                         $"{User.ErrorMessage}",
                         "Close");
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
