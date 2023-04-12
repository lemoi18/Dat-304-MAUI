using MauiApp8.Model;

namespace MauiApp8.Services.DataServices
{
    public interface IDataService
    {
        Task<List<Food>> GetFoods();
        Task<Food> GetFoodByName(string name);
    }
}
