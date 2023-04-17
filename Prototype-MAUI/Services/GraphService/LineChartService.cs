using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using Newtonsoft.Json.Linq;
using MauiApp8.Services.Health;
using MauiApp8.Services.PublishSubscribeService;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Model;
using CommunityToolkit.Mvvm.Input;

namespace MauiApp8.Services.GraphService
{
    internal partial class LineChartService<T> : ObservableObject, IChartService<T>
    {
        Publish _publish;
        IHealthService _healthService;
        [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
        ObservableCollection<Model.GlucoseInfo> glucosesChart;
        [ObservableProperty]
        ObservableCollection<Model.InsulinInfo> insulinsChart;
        public T LastPointInData { get; private set; }


        public LineChartService(Publish publish, IHealthService healthService)
        {
            _publish = publish;
            GlucosesChart = new ObservableCollection<Model.GlucoseInfo>();
            InsulinsChart = new ObservableCollection<Model.InsulinInfo>();
            _healthService = healthService;

            Task.Run(()=>_publish.HealthSub());



            _publish.GlucoseDataAvailable += (sender, e) =>
            {
                foreach (var glucose in e.GlucoseData)
                {
                    GlucosesChart.Add(glucose);
                }
               
            };

            _publish.InsulinDataAvailable += (sender, e) =>
            {
                foreach (var insulin in e.InsulinData)
                {
                    InsulinsChart.Add(insulin);

                }
                
            };
        }

        [RelayCommand]
        async Task GetHealthData()
        {
            DateTimeOffset fromDate = DateTimeOffset.UtcNow.AddDays(-1);
            DateTimeOffset toDate = DateTimeOffset.UtcNow;
            var glucose = await _healthService.ReadGlucoses(
                fromDate,
                toDate);
            var insulin = await _healthService.ReadInsulins(
                fromDate,
                toDate);

            foreach (var item in insulin)
            {
                InsulinsChart.Add(item);
            }
            foreach (var item in glucose)
            {
                GlucosesChart.Add(item);
            }

            switch (typeof(T).Name)
            {
                case nameof(HealthData):
                    LastPointInData = (T)(object)new HealthData
                    {
                        Glucose = int.TryParse(GlucosesChart.Where(g => g.Timestamp <= toDate)
                            .LastOrDefault()?.Glucose.ToString() ?? "0", out int glucoseLevel) ? glucoseLevel : 0,
                        Insulin = int.TryParse(InsulinsChart.Where(g => g.Timestamp <= toDate)
                            .LastOrDefault()?.Insulin.ToString() ?? "0", out int insulinLevel) ? insulinLevel : 0
                    };
                    break;



                default:
                    throw new NotImplementedException($"Unknown data type: {typeof(T).Name}");
            }



        }


        [Obsolete]
        public async Task<ISeries[]> GetSeries()
        {
            DateTimeOffset fromDate = DateTimeOffset.UtcNow.AddDays(-1);
            DateTimeOffset toDate = DateTimeOffset.UtcNow;
            if (GlucosesChart.Count + InsulinsChart.Count == 0)
            {
                await GetHealthData();
            }
            var glucoseValues = GlucosesChart.Select(g => g.Glucose).ToArray();
            var insulinValues = InsulinsChart.Select(i => i.Insulin).ToArray();


            return new ISeries[]
{
    new LineSeries<float>
    {
        Values = glucoseValues,
        GeometrySize = 15,
        GeometryStroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
        Name = "Glucose",
        Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 2 },
        ScalesYAt = 0,
        LegendShapeSize = 35,
        Fill = new SolidColorPaint(SKColors.Transparent), // Set Fill to transparent
        TooltipLabelFormatter = (chartPoint) =>
        {
            int index = Convert.ToInt32(chartPoint.PrimaryValue);
            GlucoseInfo glucoseInfo = GlucosesChart[index];
            string time = glucoseInfo.Timestamp.ToString("HH:mm");
            float glucoseValue = glucoseInfo.Glucose;
            return $"Time: {time}, Glucose: {glucoseValue} mg/dL";
        }
    },
    new LineSeries<double>
    {
        Values = insulinValues,
        GeometrySize = 15,
        GeometryStroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 2 },
        Name = "Insulin",
        Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 2 },
        ScalesYAt = 0,
        LegendShapeSize = 35,
        Fill = new SolidColorPaint(SKColors.Transparent), // Set Fill to transparent
        TooltipLabelFormatter = (chartPoint) =>
        {
            int index = Convert.ToInt32(chartPoint.PrimaryValue);
            InsulinInfo insulinInfo = InsulinsChart[index];
            string time = insulinInfo.Timestamp.ToString("HH:mm");
            double insulinValue = insulinInfo.Insulin;
            return $"Time: {time}, Insulin Level: {insulinValue}";
        }
    }
};



        }









    }

}
