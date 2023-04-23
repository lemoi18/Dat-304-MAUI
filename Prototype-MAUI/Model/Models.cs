using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp8.Model
{
    public class RealmUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }

    public class Meal
    {
        public int ID { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public IList<FoodEntry> FoodEntry { get; }

        public Meal()
        {
            FoodEntry = new List<FoodEntry>();
        }

        public Meal(IEnumerable<FoodEntry> foodEntries)
        {
            FoodEntry = new List<FoodEntry>(foodEntries);
        }
    }

    public class FoodEntry
    {
        public int ID { get; set; }
        public Food Food { get; set; }
        public float Amount { get; set; }

    }
    public class Food
    {
        public string Name { get; set; }
        public float Calories { get; set; }
        public float Carbohydrates { get; set; }
        public float Protein { get; set; }
        public float Fat { get; set; }
    }
    public class Configuration
    {
        public string NightscoutAPI { get; set; }
        public string NightscoutSecret { get; set; }

        public string HealthKitAPI { get; set; }
        public string HealthKitSecret { get; set; }

        public bool GPU { get; set; }
    }
    public class ExercicesInfo
    {
        public int ID { get; set; }
        public float CaloriesBurned { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
    public partial class GlucoseInfo : ObservableObject
    {
        [ObservableProperty]

        float glucose;
        [ObservableProperty]

        DateTimeOffset timestamp;
    }
    public partial class InsulinInfo : ObservableObject
    {
        [ObservableProperty]
        double insulin;
        [ObservableProperty]
        double basal;
        [ObservableProperty]
        DateTimeOffset timestamp;
    }

    public class HealthData : ObservableObject
    {
        public int Insulin { get; set; }
        public int Glucose { get; set; }
    }
}