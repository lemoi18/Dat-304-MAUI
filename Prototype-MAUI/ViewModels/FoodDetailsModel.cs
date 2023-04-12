using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiApp8.Model;
using MauiApp8.Services.DataServices;
using MauiApp8.Views;

namespace MauiApp8.ViewModel
{
    [QueryProperty(nameof(Food), nameof(Food))]

    [QueryProperty(nameof(IsEdit), nameof(IsEdit))]

    public partial class FoodDetailsModel : ObservableObject
    {
        IDataService dataService;

        [ObservableProperty]
        Food food;


        [ObservableProperty]
        string entry;

        [ObservableProperty]
        double grams;

        [ObservableProperty]
        LogFoodModel logFood;

        [ObservableProperty]
        bool isEdit;

        [ObservableProperty]
        int index;

        public FoodDetailsModel(IDataService dataService, LogFoodModel logFoodModel)
        {

            this.dataService = dataService;

            this.logFood = logFoodModel;
            IsEdit = IsEdit;
        }



        [RelayCommand]
        async Task NavigateToBackLog()
        {
            if (Grams.IsZeroOrNaN())
            {
                await Shell.Current.DisplayAlert("Error", "Please enter a valid value for grams.", "OK");
                return;
            }

            var foodVM = new FoodViewModel(Food) { Grams = Grams };

            if (IsEdit == true)
            {
                var foodToUpdate = LogFood.SelectedFoodsVM.FirstOrDefault(f => f.Name == foodVM.Name);
                if (foodToUpdate != null)
                {
                    foodToUpdate.Grams = foodVM.Grams;
                }

            }
            else
            {

                if (LogFood.SelectedFoodsVM == null)
                {
                    LogFood.SelectedFoodsVM = new MvvmHelpers.ObservableRangeCollection<FoodViewModel>();
                }

                LogFood.SelectedFoodsVM.Add(foodVM);
            }

            await Shell.Current.GoToAsync($"{nameof(FoodPage)}");

            ClearGrams();
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
