using LiveChartsCore;
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
    private async void OnFrameTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//graph");
    }
}