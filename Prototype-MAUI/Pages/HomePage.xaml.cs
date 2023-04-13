using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using MauiApp8.Services.GraphService;
using MauiApp8.ViewModel;
using SkiaSharp;

namespace MauiApp8.Views;


public partial class HomePage : ContentPage
{
    public HomePage(HomePageModel vm)
    {
        InitializeComponent();
        BindingContext = vm;

    }
    private async void OnFrameTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//graph");
    }
}