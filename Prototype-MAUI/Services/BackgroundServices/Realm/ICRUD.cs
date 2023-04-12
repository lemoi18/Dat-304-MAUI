namespace MauiApp8.Services.BackgroundServices.Realm
{
    public interface ICRUD
    {
        Task Test();
        Task AddGlucoseEntry(Realms.Realm realm, float sgv, DateTimeOffset date);
        Task AddInsulinEntry(Realms.Realm realm, double? insulin, DateTimeOffset date);
        DateTimeOffset? ReadLatestGlucoseTimestamp(Realms.Realm realm);
        DateTimeOffset? ReadLatestInsulinTimestamp(Realms.Realm realm);
        Task<int> UpdateGlucose(Realms.Realm realm, string DomainName);
        Task<int> UpdateInsulin(Realms.Realm realm, string DomainName);
        Task AddFood(Realms.Realm realm, string name, float calories, float carbohydrates, float protein, float fat);
        Model.Food ReadFood(Realms.Realm realm, string name);
        Task UpdateFood(Realms.Realm realm, string name, float calories, float carbohydrates, float protein, float fat);
        Task DeleteFood(Realms.Realm realm, string foodName);
        List<Model.Food> ReadFoods(Realms.Realm realm);
        List<Model.FoodEntry> ReadFoodEntries(Realms.Realm realm);
        Model.FoodEntry ReadFoodEntry(Realms.Realm realm, int foodEntryId);
        Task<int> CreateFoodEntry(Realms.Realm realm, string foodName, float amount);
        Task DeleteFoodEntry(Realms.Realm realm, int foodEntryId);
        List<Model.Meal> ReadMeals(Realms.Realm realm);
        Model.Meal ReadMeal(Realms.Realm realm, int mealId);
        Task<int> CreateMeal(Realms.Realm realm, List<int> foodEntryIds);
        Task DeleteMeal(Realms.Realm realm, int mealId);
        List<Model.GlucoseInfo> ReadGlucoses(Realms.Realm realm, DateTimeOffset fromDate, DateTimeOffset toDate);
        List<Model.InsulinInfo> ReadInsulins(Realms.Realm realm, DateTimeOffset fromDate, DateTimeOffset toDate);
    }
}
