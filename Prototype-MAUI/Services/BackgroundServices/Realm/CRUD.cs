using static MauiApp8.Services.BackgroundServices.Realm.Utils;
using static MauiApp8.Services.BackgroundServices.Nightscout;
using MauiApp8.Model;

namespace MauiApp8.Services.BackgroundServices.Realm
{
    internal class CRUD : ICRUD
    {
        public async Task Test()
        {
            string DomainName = "https://oskarnightscoutweb1.azurewebsites.net";

            Console.WriteLine("Running CRUD tests...");

            var utils = new Utils();

            using Realms.Realm realm = utils.RealmCreate();

            Console.WriteLine("Testing glucose and insulin:");
            //await UpdateGlucose(realm, DomainName);
            //await UpdateInsulin(realm, DomainName);

            DateTimeOffset fromDate = DateTimeOffset.UtcNow.AddDays(-1);
            DateTimeOffset toDate = DateTimeOffset.UtcNow;

            Console.WriteLine($"Reading glucose data from {fromDate} to {toDate}:");
            List<GlucoseInfo> glucoseData = ReadGlucoses(realm, fromDate, toDate);
            foreach (var glucoseEntry in glucoseData)
            {
                Console.WriteLine($"Timestamp: {glucoseEntry.Timestamp}, Glucose: {glucoseEntry.Glucose}");
            }

            Console.WriteLine($"Reading insulin data from {fromDate} to {toDate}:");
            List<InsulinInfo> insulinData = ReadInsulins(realm, fromDate, toDate);
            foreach (var insulinEntry in insulinData)
            {
                Console.WriteLine($"Timestamp: {insulinEntry.Timestamp}, Insulin: {insulinEntry.Insulin}");
            }


            Console.WriteLine("Testing Food:");

            await AddFood(realm, "Example Food", 100, 10, 5, 3);
            await AddFood(realm, "Example Food 2", 200, 20, 10, 6);
            var allFoods = ReadFoods(realm);
            Console.WriteLine("All foods in the database:");
            foreach (var food in allFoods)
            {
                Console.WriteLine($"Name: {food.Name}, Calories: {food.Calories}, Carbohydrates: {food.Carbohydrates}, Protein: {food.Protein}, Fat: {food.Fat}");
            }

            Food exampleFood = ReadFood(realm, "Example Food");
            Console.WriteLine($"GetFood: Name: {exampleFood.Name}, Calories: {exampleFood.Calories}, Carbohydrates: {exampleFood.Carbohydrates}, Protein: {exampleFood.Protein}, Fat: {exampleFood.Fat}");

            await UpdateFood(realm, "Example Food", 120, 12, 6, 4);
            Console.WriteLine("Updated Food:");
            Food updatedFood = ReadFood(realm, "Example Food");
            Console.WriteLine($"Name: {updatedFood.Name}, Calories: {updatedFood.Calories}, Carbohydrates: {updatedFood.Carbohydrates}, Protein: {updatedFood.Protein}, Fat: {updatedFood.Fat}");


            Console.WriteLine("Testing FoodEntry:");
            int foodEntryId1 = await CreateFoodEntry(realm, "Example Food", 100);
            int foodEntryId2 = await CreateFoodEntry(realm, "Example Food 2", 200);

            Console.WriteLine("Food Entries in the database:");
            var allFoodEntries = ReadFoodEntries(realm);
            foreach (var entry in allFoodEntries)
            {
                Console.WriteLine($"ID: {entry.ID}, Food: {entry.Food.Name}, Amount: {entry.Amount}");
            }

            FoodEntry readFoodEntry = ReadFoodEntry(realm, foodEntryId1);
            Console.WriteLine($"ReadFoodEntry: ID: {readFoodEntry.ID}, Food: {readFoodEntry.Food.Name}, Amount: {readFoodEntry.Amount}");


            Console.WriteLine("Testing Meal:");
            int mealId = await CreateMeal(realm, new List<int> { foodEntryId1, foodEntryId2 });

            Console.WriteLine("Meals in the database:");
            var allMeals = ReadMeals(realm);
            foreach (var meal in allMeals)
            {
                Console.WriteLine($"ID: {meal.ID}, Timestamp: {meal.Timestamp}");
                Console.WriteLine("Food Entries in Meal:");
                foreach (var entry in meal.FoodEntry)
                {
                    Console.WriteLine($"  ID: {entry.ID}, Food: {entry.Food.Name}, Amount: {entry.Amount}");
                }
            }
            Meal readMeal = ReadMeal(realm, mealId);
            Console.WriteLine($"ReadMeal: ID: {readMeal.ID}, Timestamp: {readMeal.Timestamp}");


            Console.WriteLine("Cleaning up test data...");
            await DeleteFoodEntry(realm, foodEntryId1);
            await DeleteFoodEntry(realm, foodEntryId2);
            await DeleteMeal(realm, mealId);
            await DeleteFood(realm, "Example Food");
            await DeleteFood(realm, "Example Food 2");
        }

        public async Task MockFood(Realms.Realm realm)
        {
            await AddFood(realm, "Example Food", 100, 10, 5, 3);
            await AddFood(realm, "Example Food 2", 200, 20, 10, 6);
            await AddFood(realm, "Example Food 3", 300, 30, 15, 9);
            var allFoods = ReadFoods(realm);
        }

        public async Task AddGlucoseEntry(Realms.Realm realm, float sgv, DateTimeOffset date)
        {
            try
            {
                string dbFullPath = realm.Config.DatabasePath;
                var GlucoseEntry = new GlucoseInfo { Glucose = sgv, Timestamp = date };
                await realm.WriteAsync(() =>
                {
                    realm.Add(GlucoseEntry);
                });

                realm.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task AddInsulinEntry(Realms.Realm realm, double? insulin, double? basal, DateTimeOffset date)
        {
            try
            {
                string dbFullPath = realm.Config.DatabasePath;
                var InsulinEntry = new InsulinInfo { Insulin = (double)insulin, Basal= (double)basal, Timestamp = date };
                await realm.WriteAsync(() =>
                {
                    realm.Add(InsulinEntry);
                });

                realm.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public DateTimeOffset? ReadLatestGlucoseTimestamp(Realms.Realm realm)
        {
            var objects = realm.All<GlucoseInfo>();
            DateTimeOffset? maxDateTime = objects.OrderByDescending(item => item.Timestamp).FirstOrDefault()?.Timestamp;
            if (maxDateTime == null)
            {
                Console.WriteLine("The glucose List is empty");
                DateTimeOffset utcOffset = DateTimeOffset.UtcNow;
                realm.Dispose();
                return utcOffset.AddMonths(-1);
            }
            else
            {
                Console.WriteLine("The glucose List is not empty");
                DateTimeOffset dateTimeOffset = maxDateTime?.DateTime ?? DateTimeOffset.MinValue;
                TimeZoneInfo norwayTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

                // Convert the UTC time to Norwegian time
                DateTimeOffset norwayTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.UtcDateTime, norwayTimeZone);
                return norwayTime;
            }

        }

        public float? ReadLatestGlucoseValue(Realms.Realm realm)
        {
            var objects = realm.All<GlucoseInfo>();
            float? Value = objects.OrderByDescending(item => item.Timestamp).FirstOrDefault()?.Glucose;
            if (Value == null)
            {
                Console.WriteLine("The glucose List is empty");

                return 0;
            }
            else
            {
                
                return Value;
            }

        }

        public DateTimeOffset? ReadLatestInsulinTimestamp(Realms.Realm realm)
        {
            var objects = realm.All<InsulinInfo>();

            DateTimeOffset? maxDateTime = objects.OrderByDescending(item => item.Timestamp).FirstOrDefault()?.Timestamp;
            if (maxDateTime == null)
            {
                Console.WriteLine("The insulin List is empty");
                DateTimeOffset utcOffset = DateTimeOffset.UtcNow;
                realm.Dispose();
                return utcOffset.AddMonths(-1);
            }
            else
            {
                Console.WriteLine("The insulin List is not empty");
                DateTimeOffset dateTimeOffset = maxDateTime?.DateTime ?? DateTimeOffset.MinValue;
                TimeZoneInfo norwayTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

                // Convert the UTC time to Norwegian time
                DateTimeOffset norwayTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.UtcDateTime, norwayTimeZone);
                return norwayTime;
            }


        }

        public async Task<int> UpdateGlucose(Realms.Realm realm, string DomainName)
        {
            DateTimeOffset? utcStart = ReadLatestGlucoseTimestamp(realm);
            if (utcStart.HasValue == false)
            {
                return -1;
            }

            DateTimeOffset utcStartPlus = ((DateTimeOffset)utcStart).AddMinutes(5);
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo norwegianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime utcEnd = TimeZoneInfo.ConvertTimeFromUtc(utcTime, norwegianTimeZone);

            List<Model.GlucoseAPI> Items;
            Items = await Nightscout.GetGlucose(DomainName, utcStartPlus.ToString("yyyy-MM-ddTHH:mm:ss"), utcEnd.ToString("yyyy-MM-ddTHH:mm:ss"));
            Console.WriteLine("Adding " + Items.Count + " glucose entries... ");
            foreach (Model.GlucoseAPI obj in Items)
            {
                await AddGlucoseEntry(realm, obj.sgv, obj.dateString);
            }
            return 200;
        }

       

        public List<GlucoseInfo> ReadGlucoses(Realms.Realm realm, DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            try
            {
                var glucoseList = realm.All<GlucoseInfo>()
                    .Where(g => g.Timestamp >= fromDate && g.Timestamp <= toDate)
                    .OrderBy(g => g.Timestamp)
                    .ToList();

                return glucoseList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public List<InsulinInfo> ReadInsulins(Realms.Realm realm, DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            try
            {
                var insulinList = realm.All<InsulinInfo>()
                    .Where(i => i.Timestamp >= fromDate && i.Timestamp <= toDate)
                    .OrderBy(i => i.Timestamp)
                    .ToList();

                return insulinList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public List<Food> ReadFoods(Realms.Realm realm)
        {
            try
            {
                var foodList = realm.All<Food>().ToList();
                return foodList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public Food ReadFood(Realms.Realm realm, string name)
        {
            try
            {
                var food = realm.Find<Food>(name);

                if (food == null)
                {
                    Console.WriteLine($"Food with name {name} not found in the database.");
                    return null;
                }

                return food;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public async Task AddFood(Realms.Realm realm, string name, float calories, float carbohydrates, float protein, float fat)
        {
            try
            {
                var foodItem = new Food
                {
                    Name = name,
                    Calories = calories,
                    Carbohydrates = carbohydrates,
                    Protein = protein,
                    Fat = fat
                };

                await realm.WriteAsync(() =>
                {
                    realm.Add(foodItem);
                });

                realm.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task UpdateFood(Realms.Realm realm, string name, float calories, float carbohydrates, float protein, float fat)
        {
            try
            {
                var food = realm.Find<Food>(name);

                if (food != null)
                {
                    await realm.WriteAsync(() =>
                    {
                        food.Calories = calories;
                        food.Carbohydrates = carbohydrates;
                        food.Protein = protein;
                        food.Fat = fat;
                    });
                    realm.Refresh();
                }
                else
                {
                    Console.WriteLine($"Food with name {name} not found in the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task DeleteFood(Realms.Realm realm, string foodName)
        {
            try
            {
                var food = realm.Find<Food>(foodName);

                if (food != null)
                {
                    await realm.WriteAsync(() =>
                    {
                        realm.Remove(food);
                    });
                    realm.Refresh();
                }
                else
                {
                    Console.WriteLine($"Food with name {foodName} not found in the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public List<FoodEntry> ReadFoodEntries(Realms.Realm realm)
        {
            var foodEntryList = realm.All<FoodEntry>().ToList();
            return foodEntryList;
        }

        public FoodEntry ReadFoodEntry(Realms.Realm realm, int foodEntryId)
        {
            var foodEntry = realm.Find<FoodEntry>(foodEntryId);
            return foodEntry != null ? foodEntry : null;
        }

        private int GetNextFoodEntryId(Realms.Realm realm)
        {
            var maxId = realm.All<FoodEntry>().OrderByDescending(e => e.ID).FirstOrDefault()?.ID ?? 0;
            return maxId + 1;
        }

        public async Task<int> CreateFoodEntry(Realms.Realm realm, string foodName, float amount)
        {
            try
            {
                var food = realm.Find<Food>(foodName);

                if (food != null)
                {
                    var foodEntry = new FoodEntry
                    {
                        ID = GetNextFoodEntryId(realm),
                        Food = food,
                        Amount = amount
                    };

                    await realm.WriteAsync(() =>
                    {
                        realm.Add(foodEntry);
                    });
                    realm.Refresh();
                    return foodEntry.ID;
                }
                else
                {
                    Console.WriteLine($"Food with name {foodName} not found in the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return -1;
        }

        public async Task DeleteFoodEntry(Realms.Realm realm, int foodEntryId)
        {
            try
            {
                var foodEntry = realm.Find<FoodEntry>(foodEntryId);

                if (foodEntry != null)
                {
                    await realm.WriteAsync(() =>
                    {
                        realm.Remove(foodEntry);
                    });
                    realm.Refresh();
                }
                else
                {
                    Console.WriteLine($"FoodEntry with ID {foodEntryId} not found in the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        public List<Meal> ReadMeals(Realms.Realm realm)
        {
            var mealList = realm.All<Meal>().ToList();
            return mealList;
        }

        public Meal ReadMeal(Realms.Realm realm, int mealId)
        {
            var meal = realm.Find<Meal>(mealId);
            return meal != null ? meal : null;
        }

        private int GetNextMealId(Realms.Realm realm)
        {
            var maxId = realm.All<Meal>().OrderByDescending(m => m.ID).FirstOrDefault()?.ID ?? 0;
            return maxId + 1;
        }

        public async Task<int> CreateMeal(Realms.Realm realm, List<int> foodEntryIds)
        {
            try
            {
                var foodEntries = foodEntryIds.Select(id => realm.Find<FoodEntry>(id)).ToList();

                if (foodEntries.Any(entry => entry == null))
                {
                    Console.WriteLine("One or more food entries not found in the database.");
                }

                var meal = new Meal
                {
                    ID = GetNextMealId(realm),
                    Timestamp = DateTimeOffset.UtcNow
                };

                foreach (var entry in foodEntries)
                {
                    meal.FoodEntry.Add(entry);
                }

                await realm.WriteAsync(() =>
                {
                    realm.Add(meal);
                });

                Console.WriteLine("Meal with ID ", meal.ID, " added to database");
                return meal.ID;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            return -1;
        }

        public async Task DeleteMeal(Realms.Realm realm, int mealId)
        {
            try
            {
                var meal = realm.Find<Meal>(mealId);

                if (meal != null)
                {
                    await realm.WriteAsync(() =>
                    {
                        realm.Remove(meal);
                    });
                    realm.Refresh();
                }
                else
                {
                    Console.WriteLine($"Meal with ID {mealId} not found in the database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task<List<GlucoseInfo>> ReadAllGlucose(Realms.Realm realm)
        {
            List<GlucoseInfo> glucoselist = realm.All<GlucoseInfo>().ToList();
            return glucoselist;
        }
        public async Task<List<ExercicesInfo>> ReadAllExercice(Realms.Realm realm)
        {
            List<ExercicesInfo> exercicelist = realm.All<ExercicesInfo>().ToList();
            return exercicelist;
        }

        public async void ConsoleAllExercice(Realms.Realm realm)
        {
            List<ExercicesInfo> exercicelist = realm.All<ExercicesInfo>().ToList();
            Console.WriteLine("Starting looking at exercice...");
            foreach (var exercice in exercicelist)
            {
                Console.WriteLine($"Steps: {exercice.Steps}, Start Time: {exercice.Start}, End Time: {exercice.End}");
            }

        }

        public async Task AddExerciceEntry(Realms.Realm realm, float steps, DateTimeOffset start, DateTimeOffset end) 
        {
            using (var trans = realm.BeginWrite())
            {
                var newObj = new ExercicesInfo
                {
                    Steps = steps,
                    Start = start,
                    End = end
                };
                realm.Add(newObj);
                trans.Commit();
            }
            await Task.CompletedTask;
        }

        public async Task DeleteExerciceEntriesAfterDate(Realms.Realm realm, DateTimeOffset date) 
        {
            var objectsToDelete = realm.All<ExercicesInfo>().Where(obj => obj.Start > date);
            int count = objectsToDelete.Count();
            Console.WriteLine($"Number of objects to delete: {count}");

            // begin a write transaction to delete the objects
            if (count > 0)
            {
                await realm.WriteAsync(() =>
                {
                    realm.RemoveRange(objectsToDelete);
                });
                realm.Refresh();
            }
            else
            {
                Console.WriteLine($"No objects to delete");
            }

           // await Task.CompletedTask;
        }


        public async Task DeleteAllExercice(Realms.Realm realm) {
            var allObjects = realm.All<ExercicesInfo>();
            int count = allObjects.Count();
            Console.WriteLine($"Number of objects to delete: {count}");

            // begin a write transaction to delete all objects
            if (count > 0)
            {
                await realm.WriteAsync(() =>
                {
                    realm.RemoveRange(allObjects);
                });
                realm.Refresh();
            }
            else
            {
                Console.WriteLine($"No objects to delete");
            }
        }
    }
}
