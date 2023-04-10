using System;
using Realms;
using Realms.Exceptions;
using MauiApp8.Services.BackgroundServices.Realm; // Add this namespace

namespace MauiApp8.Services.DBService
{
    public class CreateDB
    {
        public static Realm RealmCreate()
        {
            string pathToDb = $"{AppDomain.CurrentDomain.BaseDirectory}";
            var config = new RealmConfiguration(pathToDb + "/my.realm")
            {
                IsReadOnly = false,
                ObjectClasses = new Type[] // Add this line to specify the ObjectClasses explicitly
                {
                    typeof(RealmUser),
                    typeof(FoodEntries),
                    typeof(FoodEntry),
                    typeof(Food),
                    typeof(Configuration),
                    typeof(ExercicesInfo),
                    typeof(GlucoseInfo),
                    typeof(InsulinInfo)
                }
            };

            Realm localRealm;
            try
            {
                localRealm = Realm.GetInstance(config);
                return localRealm;
            }
            catch (RealmFileAccessErrorException ex)
            {
                Console.WriteLine($@"Error creating or opening the realm file. A! {ex.Message}");
                return null;
            }
        }
    }
}
