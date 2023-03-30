namespace MauiApp8;
using Views;

public partial class LoginShell : Shell
{
    public LoginShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
       

    }
}
