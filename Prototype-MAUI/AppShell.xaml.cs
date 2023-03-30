namespace MauiApp8;
using Views;
using Model;

public partial class AppShell : Shell
{

    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(FoodPage), typeof(FoodPage));
        Routing.RegisterRoute(nameof(FoodDetailsPage), typeof(FoodDetailsPage));
        Routing.RegisterRoute(nameof(FoodItemView), typeof(FoodItemView));


    }
}
