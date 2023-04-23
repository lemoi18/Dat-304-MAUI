namespace MauiApp8.Services.Food
{
    public interface IFoodService
    {
        Task MockFood();
        Task<List<Model.Food>> GetFoods();
        Task<Model.Food> GetFood(string name);
        Task AddFood(string name, float calories, float carbohydrates, float protein, float fat);
        Task UpdateFood(string name, float calories, float carbohydrates, float protein, float fat);
        Task DeleteFood(string foodName);
        Task<List<Model.FoodEntry>> GetFoodEntries();
        Task<Model.FoodEntry> GetFoodEntry(int foodEntryId);
        Task<int> CreateFoodEntry(string foodName, float amount);
        Task DeleteFoodEntry(int foodEntryId);
        Task<List<Model.Meal>> GetMeals();
        Task<Model.Meal> GetMeal(int mealId);
        Task<int> CreateMeal(List<int> foodEntryIds);
        Task DeleteMeal(int mealId);
    }
}
