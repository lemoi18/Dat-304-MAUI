using LiveChartsCore;
using MauiApp8.Model;
using System.Collections.ObjectModel;

namespace MauiApp8.Services.GraphService
{
    public interface IChartService<T>
    {
        
        event EventHandler DataChanged;
        event EventHandler NotifyDataChanged;
        bool IsDataChanged { get; set; }
        T LastPointInData { get; }

        Task<ISeries> AddBasalSeries();
        Task<ISeries> AddInsulinSeries();
        Task<ISeries> AddGlucosesSeries();
        ObservableCollection<GlucoseInfo> GlucosesChart { get; }
        ObservableCollection<InsulinInfo> InsulinsChart { get; }
    }
}
