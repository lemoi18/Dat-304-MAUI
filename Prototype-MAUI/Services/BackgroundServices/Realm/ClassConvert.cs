namespace MauiApp8.Services.BackgroundServices.Realm
{
    internal class ClassConvert
    {
        public static Model.RealmUser ToModel(RealmUser realmUser)
        {
            return new Model.RealmUser
            {
                Id = realmUser.Id,
                Name = realmUser.Name,
                Email = realmUser.Email,
                Age = realmUser.Age
            };
        }

        public static Model.Meal ToModel(Realm.Meal realmMeal)
        {
            var foodEntries = new List<Model.FoodEntry>();
            foreach (var entry in realmMeal.FoodEntry)
            {
                foodEntries.Add(ToModel(entry));
            }
            return new Model.Meal(foodEntries)
            {
                ID = realmMeal.ID,
                Timestamp = realmMeal.Timestamp
            };
        }



        public static Model.FoodEntry ToModel(Realm.FoodEntry realmFoodEntry)
        {
            return new Model.FoodEntry
            {
                ID = realmFoodEntry.ID,
                Food = ToModel(realmFoodEntry.Food),
                Amount = realmFoodEntry.Amount
            };
        }

        public static Model.Food ToModel(Realm.Food realmFood)
        {
            return new Model.Food
            {
                Name = realmFood.Name,
                Calories = realmFood.Calories,
                Carbohydrates = realmFood.Carbohydrates,
                Protein = realmFood.Protein,
                Fat = realmFood.Fat
            };
        }

        public static Model.Configuration ToModel(Realm.Configuration realmConfiguration)
        {
            return new Model.Configuration
            {
                NightscoutAPI = realmConfiguration.NightscoutAPI,
                NightscoutSecret = realmConfiguration.NightscoutSecret,
                HealthKitAPI = realmConfiguration.HealthKitAPI,
                HealthKitSecret = realmConfiguration.HealthKitSecret,
                GPU = realmConfiguration.GPU
            };
        }

        public static Model.ExercicesInfo ToModel(Realm.ExercicesInfo realmExercicesInfo)
        {
            return new Model.ExercicesInfo
            {
                
                Steps = realmExercicesInfo.Steps,
                Start = realmExercicesInfo.Start,
                End = realmExercicesInfo.End
            };
        }

        public static Model.GlucoseInfo ToModel(Realm.GlucoseInfo realmGlucoseInfo)
        {
            return new Model.GlucoseInfo
            {
                Glucose = realmGlucoseInfo.Glucose,
                Timestamp = realmGlucoseInfo.Timestamp
            };
        }

        public static Model.InsulinInfo ToModel(Realm.InsulinInfo realmInsulinInfo)
        {
            return new Model.InsulinInfo
            {
                Insulin = realmInsulinInfo.Insulin,
                Timestamp = realmInsulinInfo.Timestamp
            };
        }
    }
}
