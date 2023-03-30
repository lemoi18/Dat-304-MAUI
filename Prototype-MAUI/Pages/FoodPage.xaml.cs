using MauiApp8.ViewModel;
using MauiApp8.Model;

namespace MauiApp8.Views;

public partial class FoodPage : ContentPage
{
    public FoodPage(LogFoodModel vm)
    {
        InitializeComponent();

        BindingContext = vm;

    }




    public void OnNavigated(Dictionary<string, string> queryParameters)
    {
        if (queryParameters.ContainsKey("Food"))
        {
            // Get the value of the "Food" parameter
            string foodParam = queryParameters["Food"];

            // TODO: Convert the foodParam string to your Food object and update your view model accordingly
        }
    }

}