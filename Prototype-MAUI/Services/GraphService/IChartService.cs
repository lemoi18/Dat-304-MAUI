using LiveChartsCore;
using MauiApp8.Model;
using System.Collections.ObjectModel;

namespace MauiApp8.Services.GraphService
{
    public interface IChartService<T>
    {
        Task<ISeries[]> GetSeries();
        T LastPointInData { get; }
        ObservableCollection<GlucoseInfo> GlucosesChart { get; }
        ObservableCollection<InsulinInfo> InsulinsChart { get; }
    }
}
