using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp8.Model;
using MauiApp8.Views;

namespace MauiApp8.ViewModel
{
    public partial class FoodViewModel : ObservableObject
    {
        public Food Food { get; set; }
        public string Name => Food?.Name;

        public float Carbohydrates => (float)Food?.Carbohydrates;

        [ObservableProperty]
        double grams;

        [ObservableProperty]
        bool isEdit;

        [ObservableProperty]
        bool isSelected;

        public int CarbohydratesPerKg => (int)(Carbohydrates * Grams / 1000);

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

            await Shell.Current.GoToAsync($"{nameof(FoodDetailsPage)}", parameters);
        }
    }
}
