using CommunityToolkit.Mvvm.Input;
using MauiApp8.Views;
using MauiApp8.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.BackgroundServices;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;


namespace MauiApp8.ViewModel
{
    public partial class GraphPageModel : ObservableObject
    {
    public <ISeries[] Series { get; set; }
        = new ISeries[]
        {
        new LineSeries<double>
            {
            Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
            Fill = null
            }
        };
    }
}
