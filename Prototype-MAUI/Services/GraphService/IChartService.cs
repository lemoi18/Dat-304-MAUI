using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using MauiApp8.Model;
using System.Collections.ObjectModel;

namespace MauiApp8.Services.GraphService
{
    public interface IChartService<T>
    {
        
      
        T LastPointInData { get; }

        DateTimeOffset FromDate { get; set; }
        DateTimeOffset ToDate { get; set; }
        Task<ISeries> AddBasalSeries();
        Task<ISeries> AddInsulinSeries();
        Task<ISeries> AddGlucosesSeries();
        ObservableCollection<GlucoseInfo> GlucosesChart { get; }
        ObservableCollection<InsulinInfo> InsulinsChart { get; }
    }
}
