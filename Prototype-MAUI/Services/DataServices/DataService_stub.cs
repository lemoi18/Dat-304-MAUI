using MauiApp8.Model;

namespace MauiApp8.Services.DataServices
{
    internal class FoodService_stub : IDataService
    {

        private readonly List<Food> _mockFoods;


        public FoodService_stub()
        {
            _mockFoods = new List<Food>
        {
            new Food
            {
                Name = "Grilled Chicken",
                Carbohydrates = 0,
                Protein = 31,
                Fat = (float)3.6
            },
            new Food
            {
                Name = "Brown Rice",
                Carbohydrates = 45,
                Protein = 5,
                Fat = (float) 1.8
            },
            new Food
            {
                Name = "Greek Salad",
                Carbohydrates = 11,
                Protein = 4,
                Fat = 16
            },
            new Food
            {
                Name = "Protein Shake",
                Carbohydrates = 30,
                Protein = 25,
                Fat = 3
            }
        };

        }
        public Task<List<Food>> GetFoods()
        {
            return Task.FromResult<List<Food>>(_mockFoods);
        }

        public Task<Food> GetFoodByName(string name)
        {
            Food food = _mockFoods.FirstOrDefault(food => food.Name == name);
            return Task.FromResult(food);
        }
    }
}
