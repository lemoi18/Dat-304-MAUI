using MauiApp8.ViewModel;
using System.Diagnostics;

namespace MauiApp8.Views
{
    public partial class GraphPage : ContentPage
    {
        public GraphPage(GraphPageModel vm)
        {
            InitializeComponent();

            BindingContext = vm;

        }


    }
}
