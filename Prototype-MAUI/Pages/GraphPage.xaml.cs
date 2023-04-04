using MauiApp8.ViewModel;
using Microcharts;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Diagnostics;

namespace MauiApp8.Views
{
    public partial class GraphPage : ContentPage
    {
        private GraphPageModel viewModel;

        public GraphPage()
        {
            InitializeComponent();

            viewModel = (GraphPageModel)BindingContext;
            chartCanvas.PaintSurface += OnCanvasViewPaintSurface;
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
                using (SKCanvas canvas = args.Surface.Canvas)
                {
                    canvas.Clear(SKColors.White);
                    Debug.WriteLine("Canvas cleared.");

                    if (viewModel != null && viewModel.LineChart != null)
                    {
                        Debug.WriteLine("Drawing chart...");
                        Chart chart = viewModel.LineChart;
                        // Set the label to horiz
 
                        // Set the size of the canvas
                        chart.Draw(canvas, args.Info.Width, args.Info.Height);

                        Debug.WriteLine("Chart drawn.");
                    }
                    else
                    {
                        Debug.WriteLine("Chart or ViewModel is null.");
                    }
                }
        }
    }
}
