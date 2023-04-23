using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp8.Model;
using MauiApp8.Views;
using MauiApp8.Services.Food;

namespace MauiApp8.ViewModel
{
    [QueryProperty(nameof(Food), nameof(Food))]
    [QueryProperty(nameof(IsEdit), nameof(IsEdit))]

    public partial class FoodDetailsModel : ObservableObject
    {
        IFoodService foodService;

        [ObservableProperty]
        Food food;

        [ObservableProperty]
        string entry;

        private double _grams;
        public double Grams
        {
            get => _grams;
            set
            {
                if (SetProperty(ref _grams, value))
                {
                    CarbohydratesInGrams = (int)(food.Carbohydrates * _grams / 1000);
                }
            }
        }

        private double _carbohydratesInGrams;
        public double CarbohydratesInGrams
        {
            get => _carbohydratesInGrams;
            set => SetProperty(ref _carbohydratesInGrams, value);
        }

        [ObservableProperty]
        LogFoodModel logFood;

        [ObservableProperty]
        bool isEdit;

        [ObservableProperty]
        int index;

        public FoodDetailsModel(IFoodService foodService, LogFoodModel logFoodModel)
        {
            this.foodService = foodService;
            this.logFood = logFoodModel;
            IsEdit = IsEdit;
        }

        [RelayCommand]
        async Task NavigateToBackLog()
        {
            try
            {
                if (Grams.IsZeroOrNaN())
                {
                    await Shell.Current.DisplayAlert("Error", "Please enter a valid value for grams.", "OK");
                    return;
                }

                var foodVM = new FoodViewModel(Food) { Grams = Grams };

                if (IsEdit == true)
                {
                    var foodToUpdate = logFood.SelectedFoodsVM.FirstOrDefault(f => f.Name == foodVM.Name);
                    if (foodToUpdate != null)
                    {
                        foodToUpdate.Grams = foodVM.Grams;
                    }
                }
                else
                {
                    if (logFood.SelectedFoodsVM == null)
                    {
                        logFood.SelectedFoodsVM = new MvvmHelpers.ObservableRangeCollection<FoodViewModel>();
                    }

                    logFood.SelectedFoodsVM.Add(foodVM);
                }

                await Shell.Current.GoToAsync($"{nameof(FoodPage)}");
                ClearGrams();
            }
            catch (Exception error)
            {
                Console.WriteLine("NavigateToBackLog: ", error);
            }
        }

        public void ClearGrams()
        {
            Grams = 0;
        }

        [RelayCommand]
        async Task NavigateBack()
        {
            await Shell.Current.GoToAsync("..");
            ClearGrams();
        }

        [RelayCommand]
        Task AddSelectedFood() => Shell.Current.GoToAsync("..");
    }
}
