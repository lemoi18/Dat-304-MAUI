using Microsoft.Maui.Controls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MauiApp8.Model;
using MauiApp8.Views;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MauiApp8.ViewModel
{
    public partial class FoodViewModel : ObservableObject
    {
        public INavigation Navigation { get; set; }
        public Food Food { get; set; }
        public string Name => Food?.Name;

        public float Carbohydrates => (float)Food?.Carbohydrates;

        private int _grams;
        public int Grams
        {
            get => _grams;
            set => SetProperty(ref _grams, value);
        }

        private bool _isEdit;
        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public int CarbohydratesPerKg => (int)(Carbohydrates * Grams / 1000);

        public FoodViewModel(Food food)
        {
            Food = food;
        }

        public RelayCommand<FoodViewModel> NavigateToDetailsCommand => new RelayCommand<FoodViewModel>(selectedFoodViewModel => NavigateToDetails(selectedFoodViewModel));

        private void NavigateToDetails(FoodViewModel selectedFoodViewModel)
        {
            if (Navigation != null)
            {
                var mainPage = (NavigationPage)Application.Current.MainPage;
                var logFoodModel = (LogFoodModel)mainPage.BindingContext;
                Navigation.PushAsync(new FoodDetailsPage(logFoodModel, selectedFoodViewModel));
            }
        }

    }
}
