using MauiApp8.Services.BackgroundServices.Realm;
using MauiApp8.Services.Food;
using MauiApp8.ViewModel;
using MauiApp8.Services.Authentication;

namespace MauiApp8.Views;


public partial class FoodDetailsPage : ContentPage
{
    public FoodDetailsPage(LogFoodModel logFoodModel, FoodViewModel selectedFoodViewModel)
    {
        InitializeComponent();

        var foodService = logFoodModel.FoodService;
        BindingContext = new FoodDetailsModel(foodService, logFoodModel, selectedFoodViewModel);
    }



    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    {
        ((Entry)sender).Unfocus();
    }


}