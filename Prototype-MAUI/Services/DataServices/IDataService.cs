namespace MauiApp8.Services.DataServices
{
    public interface IFoodService
    {
        Task<List<Model.Food>> GetFoods();
        Task<Model.Food> GetFoodByName(string name);
    }
}
