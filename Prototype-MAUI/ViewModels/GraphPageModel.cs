using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using MauiApp8.Services.GraphService;

namespace MauiApp8.ViewModel
{
    public partial class GraphPageModel : ObservableObject
    {
       private readonly IChartService _chartService;
        [ObservableProperty]
       private ISeries[] series;

        public GraphPageModel(IChartService chartService) 
        {
            _chartService = chartService;
            Series = _chartService.GetSeries();

        }




    }
}
