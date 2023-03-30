using MauiApp8.ViewModel;
namespace MauiApp8.Views;


public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsPageModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        
    }
}