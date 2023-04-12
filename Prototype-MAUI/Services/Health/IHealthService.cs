using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MauiApp8.Model;

namespace MauiApp8.Services.Health
{
    internal interface IHealthService
    {
        List<GlucoseInfo> ReadGlucoses(DateTimeOffset fromDate, DateTimeOffset toDate);
        List<InsulinInfo> ReadInsulins(DateTimeOffset fromDate, DateTimeOffset toDate);
        List<Food> ReadFoods();
        Food ReadFood(string name);
        Task AddFood(string name, float calories, float carbohydrates, float protein, float fat);
        Task UpdateFood(string name, float calories, float carbohydrates, float protein, float fat);
        Task DeleteFood(string foodName);
        List<FoodEntry> ReadFoodEntries();
        FoodEntry ReadFoodEntry(int foodEntryId);
        Task<int> CreateFoodEntry(string foodName, float amount);
        Task DeleteFoodEntry(int foodEntryId);
        List<Meal> ReadMeals();
        Meal ReadMeal(int mealId);
        Task<int> CreateMeal(List<int> foodEntryIds);
        Task DeleteMeal(int mealId);
    }
}
