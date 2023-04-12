using MauiApp8.ViewModel;

namespace MauiApp8.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageModel vm)
    {

        InitializeComponent();
        BindingContext = vm;
    }
}