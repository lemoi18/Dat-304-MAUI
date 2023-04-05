using MauiApp8.ViewModel;
using SkiaSharp;
using Microcharts;

namespace MauiApp8.Views;


public partial class HomePage : ContentPage
{
    private GraphPageModel graphPageModel;

    public HomePage()
    {
        InitializeComponent();
        // Instantiate GraphPageModel
        graphPageModel = new GraphPageModel();

        // Get the chart from GraphPageModel
        Chart chart = graphPageModel.LineChart;

        // Draw the chart on the canvas
        chartCanvas.PaintSurface += (sender, args) =>
        {
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear(SKColors.White);
            // if chart isnt null
            chart?.Draw(canvas, args.Info.Width, args.Info.Height);
        };

    }
}