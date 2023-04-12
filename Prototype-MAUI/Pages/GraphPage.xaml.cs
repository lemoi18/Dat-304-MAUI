using MauiApp8.ViewModel;
namespace MauiApp8.Views;


public partial class GraphPage : ContentPage
{
    public GraphPage(GraphPageModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        
    }
}