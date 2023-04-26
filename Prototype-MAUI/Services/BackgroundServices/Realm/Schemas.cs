using Realms;

namespace MauiApp8.Services.BackgroundServices.Realm
{
    public class RealmUser : RealmObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }

    public class Meal : RealmObject
    {
        [PrimaryKey]
        public int ID { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public IList<FoodEntry> FoodEntry { get; }
    }
    public class FoodEntry : RealmObject
    {
        [PrimaryKey]
        public int ID { get; set; }
        public Food Food { get; set; }
        public float Amount { get; set; }

    }
    public class Food : RealmObject
    {
        [PrimaryKey]
        public string Name { get; set; }
        public float Calories { get; set; }
        public float Carbohydrates { get; set; }
        public float Protein { get; set; }
        public float Fat { get; set; }
    }
    public class Configuration : RealmObject
    {
        public string NightscoutAPI { get; set; }
        public string NightscoutSecret { get; set; }

        public string HealthKitAPI { get; set; }
        public string HealthKitSecret { get; set; }

        public bool GPU { get; set; }
    }
    public class ExercicesInfo : RealmObject
    {
       
        public float Steps { get; set; }
        public DateTimeOffset Start { get; set; }

        public DateTimeOffset End { get; set; }
    }
    public class GlucoseInfo : RealmObject
    {
        public float Glucose { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
    public class InsulinInfo : RealmObject
    {
        public double Insulin { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}