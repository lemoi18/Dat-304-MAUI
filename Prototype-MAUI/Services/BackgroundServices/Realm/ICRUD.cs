namespace MauiApp8.Services.BackgroundServices.Realm
{
    public interface ICRUD
    {
        void Test();
        void AddGlucoseEntry(float sgv, DateTimeOffset date);
        void AddInsulinEntry(double? insulin, DateTimeOffset date);
        DateTimeOffset? ReadLatestGlucose();
        DateTimeOffset? ReadLatestInsulin();
        Task<int> UpdateGlucose(string DomainName);
        Task<int> UpdateInsulin(string DomainName);
        void AddFood(string name, float calories, float carbohydrates, float protein, float fat);
    }
}
