using CommunityToolkit.Mvvm.Input;
using MauiApp8.Views;
using MauiApp8.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using SkiaSharp;
using System.Collections.ObjectModel;
using MongoDB.Bson;

namespace MauiApp8.ViewModel
{
    public partial class GraphPageModel : ObservableObject
    {
        private Chart lineChart;
        public Chart LineChart
        {
            get { return lineChart; }
            set { SetProperty(ref lineChart, value); }
        }

        public GraphPageModel()
        {
            LineChart = new LineChart()
            {
                Entries = new ObservableCollection<ChartEntry>()
                {
                    new ChartEntry(200)
                    {
                        Label = "January",
                        ValueLabel = "200",
                        Color = SKColor.Parse("#266489")
                    },
                    new ChartEntry(400)
                    {
                        Label = "February",
                        ValueLabel = "400",
                        Color = SKColor.Parse("#68B9C0")
                    },
                    new ChartEntry(-100)
                    {
                        Label = "March",
                        ValueLabel = "-100",
                        Color = SKColor.Parse("#90D585")
                    }
                },

                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = true,
                LabelTextSize = 20,
                LabelColor = SKColors.Black,
            };

        }
    }
}
