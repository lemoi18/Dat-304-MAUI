using MauiApp8.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Id = 1,
                Name = "Grilled Chicken",
                Description = "Tender and juicy grilled chicken breast.",
                Category  = "Main Course",
                Carbohydrates = "0g",
                Protein = "31g",
                Fat = "3.6g"
            },
            new Food
            {
                Id = 2,
                Name = "Brown Rice",
                Description = "Cooked whole grain brown rice.",
                Category  = "Side Dish",
                Carbohydrates = "45g",
                Protein = "5g",
                Fat = "1.8g"
            },
            new Food
            {
                Id = 3,
                Name = "Greek Salad",
                Description = "A refreshing salad with cucumber, tomato, olives, and feta cheese.",
                Category  = "Salad",
                Carbohydrates = "11g",
                Protein = "4g",
                Fat = "16g"
            },
            new Food
            {
                Id = 4,
                Name = "Protein Shake",
                Description = "A nutritious protein shake made with whey protein, milk, and a banana.",
                Category = "Beverage",
                Carbohydrates = "30g",
                Protein = "25g",
                Fat = "3g"
            }
        };

        }
        public Task<List<Food>> GetFoods()
        {
            return Task.FromResult<List<Food>>(_mockFoods);
        }

        public async Task UpdateDetails(string newDetails, string Id)
        {

            var food = _mockFoods.FirstOrDefault(f => f.Id.ToString() == Id);
            if (food != null)
            {
                food.Description = newDetails;
                await Task.CompletedTask;
            }
            else
            {
                throw new InvalidOperationException($"Food with ID {Id} not found");
            }

        }

        public Task<Food> GetFoodById(int id)
        {
            Food food = _mockFoods.FirstOrDefault(food => food.Id == id);
            return Task.FromResult(food);
        }
    }
}
