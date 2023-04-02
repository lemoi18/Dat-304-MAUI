using CommunityToolkit.Mvvm.Input;
using MauiApp8.Views;
using MauiApp8.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Services.Authentication;

namespace MauiApp8.ViewModel
{
    public partial class SettingsPageModel : ObservableObject
    {
        IAuthenticationService authService;
        private Account _user;
        public Account User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public SettingsPageModel(IAuthenticationService _authService) 
        {
            this.authService = _authService;

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
