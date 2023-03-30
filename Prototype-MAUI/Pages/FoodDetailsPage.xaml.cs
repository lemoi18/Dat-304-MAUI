using MauiApp8.Model;
using MauiApp8.ViewModel;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace MauiApp8.Views;


public partial class FoodDetailsPage : ContentPage
{
   

    public FoodDetailsPage(FoodDetailsModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    {
        // Remove the focus from the Entry control
        ((Entry)sender).Unfocus();
    }


}