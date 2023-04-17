using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using MauiApp8.Model;
using SkiaSharp;
using System;
using System.Collections.Generic;
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
        public ChartConfiguration GetChartConfiguration(string[] strings)
        {
            var title = new LabelVisual
            {
                Text = "Health Data",
                TextSize = 48,
                Padding = new LiveChartsCore.Drawing.Padding(20),
                Paint = new SolidColorPaint(SKColors.DarkSlateGray)
            };

            // Initialize XMax, legendBackgroundPaint, and other properties that depend on data
            float xMax = _chartService.GlucosesChart.Count;
            var legendBackgroundPaint = new SolidColorPaint(new SKColor(240, 240, 240));

            var XAxes = new Axis[]
          {
                new Axis
                {
                    Name = "Time",
                    Labels = strings,
                    NameTextSize = 36,
                    MinLimit = (xMax - 100),
                    MaxLimit = xMax,
                    NamePaint = new SolidColorPaint(SKColors.Black),
                    MinStep = 1,
                    LabelsPaint = new SolidColorPaint(SKColors.Black),
                    TextSize = 28,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray) { StrokeThickness = 1 },
                    
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
                    MaxLimit = 260,
                    
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
    }
}
