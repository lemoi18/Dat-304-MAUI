using MauiApp8.Services.GraphService;
using MauiApp8.ViewModel;
namespace MauiApp8.Views;


public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        BindingContext = new GraphPageModel(new LineChartService());

    }
}