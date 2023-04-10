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
        }


    }
}
