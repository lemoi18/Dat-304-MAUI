using MauiApp8.ViewModel;
namespace MauiApp8.Views;

public partial class HomePage : ContentPage
{
    public HomePage(HomePageModel vm)
    {
        InitializeComponent();

        BindingContext = vm;

    }


}