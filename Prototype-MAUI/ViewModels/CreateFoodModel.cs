using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MauiApp8.ViewModel
{
    public partial class CreateFoodModel : ObservableObject
    {
        private readonly MauiApp8.Services.Food.IFoodService _foodService;

        public CreateFoodModel(MauiApp8.Services.Food.IFoodService foodService)
        {
            _foodService = foodService;
        }
    }
}