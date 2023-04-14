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
            var insulin = await _healthService.ReadInsulins(fromDate, toDate);
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
                GetHealthData();
            }
            var glucoseValues = GlucosesChart.Select(g => g.Glucose).ToList();
            var insulinValues = InsulinsChart.Select(i => i.Insulin).ToArray();


            return new ISeries[]
            {
        new LineSeries<float>
        {
            Values = glucoseValues,
            GeometrySize = 30,
            GeometryFill = new SolidColorPaint(SKColors.AliceBlue),
            GeometryStroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 4 },
            Fill = new SolidColorPaint(SKColors.Blue.WithAlpha(45)),
            Name = "Glucose",
            Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 10 },
            ScalesYAt = 0,
            LegendShapeSize = 35,
            // Need to fix label
            TooltipLabelFormatter = (chartPoint) => $"Time: {GlucosesChart[Convert.ToInt32(chartPoint.PrimaryValue)]}, Glucose: {chartPoint.PrimaryValue} mg/dL"
        },
        new LineSeries<double>
        {
            Values = insulinValues,
            GeometrySize = 30,
            GeometryFill = new SolidColorPaint(SKColors.MistyRose),
            GeometryStroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 4 },
            Fill = new SolidColorPaint(SKColors.Red.WithAlpha(45)),
            Name = "Insulin",
            Stroke = new SolidColorPaint(SKColors.Red) { StrokeThickness = 5 },
            ScalesYAt = 0,
            LegendShapeSize = 35,
            TooltipLabelFormatter = (chartPoint) => $"Time: {InsulinsChart[Convert.ToInt32(chartPoint.PrimaryValue)]:HH:mm}, Insulin Level: {chartPoint.PrimaryValue}"
        }
            };
            
        }









    }

}
