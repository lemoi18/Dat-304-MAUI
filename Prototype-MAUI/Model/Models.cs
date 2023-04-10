using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using Realms;
using Realms.Sync;

namespace MauiApp8.Model2
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }

    public class FoodEntries
    {

        public DateTimeOffset Timestamp { get; set; }
        public IList<FoodEntry> FoodEntry { get; }
    }
    public class FoodEntry
    {
        public Food Food { get; set; }
        public float Amout { get; set; }

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
        public float CaloriesBurned { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
    public class GlucoseInfo
    {
        public float Glucose { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
    public class InsulinInfo
    {
        public double Insulin { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}