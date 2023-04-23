using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using MauiApp8.Model;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp8.Services.GraphService
{
    

    public class ChartConfigurationProvider : IChartConfigurationProvider
    {

         IChartService<HealthData> _chartService;

        public ChartConfigurationProvider(IChartService<HealthData> chartService)
        {
            _chartService = chartService;
        }


        private string[] GenerateLabelsBasedOnInsulin(IEnumerable<InsulinInfo> data)
        {
            List<string> labels = new List<string>();
            DateTimeOffset currentDay = DateTimeOffset.MinValue;

            foreach (var dataPoint in data)
            {
                DateTimeOffset timestamp = dataPoint.Timestamp;
                string label = timestamp.ToString("HH:mm");
                if (timestamp.Date > currentDay)
                {
                    label += $" ({timestamp:MMM d})";
                    currentDay = timestamp.Date;
                }
                labels.Add(label);
            }
            return labels.ToArray();
        }

        private string[] GenerateLabelsBasedOnGlucose(IEnumerable<GlucoseInfo> data)
        {
            List<string> labels = new List<string>();
            DateTimeOffset currentDay = DateTimeOffset.MinValue;

            foreach (var dataPoint in data)
            {
                DateTimeOffset timestamp = dataPoint.Timestamp;
                string label = timestamp.ToString("HH:mm");
                if (timestamp.Date > currentDay)
                {
                    label += $" ({timestamp:MMM d})";
                    currentDay = timestamp.Date;
                }
                labels.Add(label);
            }
            return labels.ToArray();
        }

        public List<string> GenerateTimeLabels(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            List<string> timeLabels = new List<string>();

            // Convert to local time
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            DateTimeOffset fromDateLocal = TimeZoneInfo.ConvertTime(fromDate, localTimeZone);
            DateTimeOffset toDateLocal = TimeZoneInfo.ConvertTime(toDate, localTimeZone);
            

            // Start at the most recent whole hour
            DateTimeOffset recentWholeHour = fromDateLocal.AddMinutes(-fromDateLocal.Minute).AddSeconds(-fromDateLocal.Second).AddMilliseconds(-fromDateLocal.Millisecond);

            for (DateTimeOffset current = recentWholeHour; current <= toDateLocal; current = current.AddMinutes(5))
            {
                timeLabels.Add(current.ToString("HH:mm"));
            }

           
            return timeLabels;
        }


        public ChartConfiguration GetChartConfiguration(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            
            var title = new LabelVisual
            {
                Text = "Glucose and Insulin data",
                TextSize = 48,
                Padding = new LiveChartsCore.Drawing.Padding(20),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };
            


            float xMax = _chartService.GlucosesChart.Count(insulin => insulin.Timestamp >= fromDate && insulin.Timestamp <= toDate);

            // Initialize XMax, legendBackgroundPaint, and other properties that depend on data
            var legendBackgroundPaint = new SolidColorPaint(new SKColor(240, 240, 240));
            var timeLabels = GenerateTimeLabels(fromDate, toDate);
            foreach (var label in timeLabels)
            {
                Console.WriteLine(label);
            }

            var XAxes = new Axis[]
          {
                new Axis
                {
                    Name = "Time",
                    Labels = GenerateTimeLabels(fromDate,toDate),
                    NameTextSize = 36,
                    MinLimit = 0, // Set the MinLimit to 0, which corresponds to the first label's index
                    MaxLimit = timeLabels.Count - 1,
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    MinStep = 1,
                   
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    TextSize = 14, // Reduce text size
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 1 }
                    ,

                }
          };

            // Configure YAxes
            var YAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Levels",
                    NameTextSize = 36,
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    MinStep = 1,
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    TextSize = 28,
                    MinLimit = 0,
                    MaxLimit = 400,
                    
                },
            };

            // Configure LegendTextPaint
            var LegendTextPaint = new SolidColorPaint
            {
                Color = new SKColor(50, 50, 50),
                SKTypeface = SKTypeface.FromFamilyName("Courier New")
            };


            // return a ChartConfiguration object with the configuration values
            return new ChartConfiguration
            {
                XMax = xMax,
                LegendBackgroundPaint = legendBackgroundPaint,
                Title = title,
                XAxes = XAxes,
                YAxes = YAxes,
                LegendTextPaint = LegendTextPaint
            };
        }




        public ChartConfiguration GetBolosChartConfiguration(DateTimeOffset fromDate, DateTimeOffset toDate)
        {

            var title = new LabelVisual
            {
                Text = "Passive Insulin intake",
                TextSize = 48,
                Padding = new LiveChartsCore.Drawing.Padding(20),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };
            // Initialize X-axis Max

            // Get the Y-Axis max insulin data

            // Get the Y-Axis max basal data
            double  yMaxBasal = _chartService.InsulinsChart.Max(e => e.Basal);

            // Get the Y-Axis min insulin data

            // Get the Y-Axis min basal data
            double yMinBasal = _chartService.InsulinsChart.Min(e => e.Basal);

            Console.WriteLine(GenerateTimeLabels(fromDate, toDate).ToString());


            double xMax =  _chartService.InsulinsChart.Count(insulin => insulin.Timestamp >= fromDate && insulin.Timestamp <= toDate);



            var legendBackgroundPaint = new SolidColorPaint(new SKColor(240, 240, 240));

            var XAxes = new Axis[]
          {
                new Axis
                {
                    Name = "Time",
                    Labels = GenerateTimeLabels(fromDate,toDate),
                    NameTextSize = 36,
                    MinLimit = 0,
                    MaxLimit = xMax,
                    Padding = new LiveChartsCore.Drawing.Padding(10,10), // Set padding to 10 pixels on all sides
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    MinStep = 1,
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    TextSize = 14,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 1 },

                }
          };

            // Configure YAxes
            var YAxes = new Axis[]
            {
                new Axis
                {
                    Name = "Units",
                    NameTextSize = 36,
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    MinStep = 0.1,
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    TextSize = 28,
                    MinLimit = 0,
                    MaxLimit = yMaxBasal*2,

                },
            };

            // Configure LegendTextPaint
            var LegendTextPaint = new SolidColorPaint
            {
                Color = new SKColor(50, 50, 50),
                SKTypeface = SKTypeface.FromFamilyName("Courier New")
            };


            // return a ChartConfiguration object with the configuration values
            return new ChartConfiguration
            {
                XMax = (float)xMax,
                LegendBackgroundPaint = legendBackgroundPaint,
                Title = title,
                XAxes = XAxes,
                YAxes = YAxes,
                LegendTextPaint = LegendTextPaint
            };
        }






    }
}
