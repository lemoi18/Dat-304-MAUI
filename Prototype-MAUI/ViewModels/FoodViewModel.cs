using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp8.Model;
using MauiApp8.Views;
using MauiApp8.Services.DataServices;

namespace MauiApp8.ViewModel
{
    public partial class FoodViewModel : ObservableObject
    {


        public Food Food { get; set; }
        public string Name => Food?.Name;

        public int  ID => Food?.Id ?? -1;
        public string Carbohydrates => Food?.Carbohydrates;

        public string Category => Food?.Category;

        public string Description => Food?.Description;

        [ObservableProperty]
        double grams;

        [ObservableProperty]
        bool isEdit;

     


        public FoodViewModel(Food food)
        {
            Food = food;
        }

        


        [RelayCommand]
        async Task NavigateToDetails(FoodViewModel selectedFoodViewModel)
        {
            var parameters = new Dictionary<string, object>();

            if (!parameters.ContainsKey("Food"))
            {
                parameters.Add("Food", selectedFoodViewModel.Food);
            }

            await Shell.Current.GoToAsync($"{nameof(FoodDetailsPage)}",parameters);
        }





    }
}
