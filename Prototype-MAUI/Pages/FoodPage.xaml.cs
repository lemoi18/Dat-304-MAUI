using MauiApp8.ViewModel;
using MauiApp8.Model;
using Microsoft.Maui.Controls;
using System.Collections.Generic;


namespace MauiApp8.Views
{
    public partial class FoodPage : ContentPage
    {
        public FoodPage(LogFoodModel vm)
        {
            InitializeComponent();

            BindingContext = vm;

            foreach (var foodViewModel in vm.FoodVM)
            {
                foodViewModel.Navigation = this.Navigation;
            }
        }

        public void OnNavigated(Dictionary<string, string> queryParameters)
        {
            if (queryParameters.ContainsKey("Food"))
            {
                string foodParam = queryParameters["Food"];

                // TODO: Convert the foodParam string to Food object and update your view model accordingly
            }
        }
    }
}
