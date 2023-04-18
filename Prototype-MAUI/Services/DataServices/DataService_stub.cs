using MauiApp8.Model;
using MauiApp8.Services.BackgroundServices;

namespace MauiApp8.Services.DataServices
{
    internal class FoodService_stub : IFoodService
    {

        private readonly List<Model.Food> _mockFoods;
        private readonly IBackgroundService _backgroundService;


        public FoodService_stub(IBackgroundService backgroundService)
        {
            _backgroundService = backgroundService;
            _mockFoods = new List<Model.Food>
        {
            new Model.Food
            {
                Name = "Grilled Chicken",
                Carbohydrates = 0,
                Protein = 31,
                Fat = (float)3.6
            },
            new Model.Food
            {
                Name = "Brown Rice",
                Carbohydrates = 45,
                Protein = 5,
                Fat = (float) 1.8
            },
            new Model.Food
            {
                Name = "Greek Salad",
                Carbohydrates = 11,
                Protein = 4,
                Fat = 16
            },
            new Model.Food
            {
                Name = "Protein Shake",
                Carbohydrates = 30,
                Protein = 25,
                Fat = 3
            }
        };

        }
        public Task<List<Model.Food>> GetFoods()
        {
            return Task.FromResult<List<Model.Food>>(_mockFoods);
        }

        public Task<Model.Food> GetFoodByName(string name)
        {
            Model.Food food = _mockFoods.FirstOrDefault(food => food.Name == name);
            return Task.FromResult(food);
        }
    }
}
