using CommunityToolkit.Mvvm.Input;
using MauiApp8.Views;
using MauiApp8.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.BackgroundServices;

namespace MauiApp8.ViewModel
{
    public partial class GraphPageModel : ObservableObject
    {
        IAuthenticationService authService;
        private Account _user;
        public Account User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        IBackgroundService backgroundService;

        public GraphPageModel(IAuthenticationService _authService, IBackgroundService _backgroundService) 
        {
            this.authService = _authService;
            backgroundService = _backgroundService;

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
