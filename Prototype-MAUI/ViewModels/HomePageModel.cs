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
        CreateDB db;

        public HomePageModel(IAuthenticationService authService, CreateDB db)
        {

            this.db = db;
            this.authService = authService;
            _user = authService.User;
            // Access the CurrentUser property to get the user object
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

