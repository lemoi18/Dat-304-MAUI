﻿using MauiApp8.Model;
using MauiApp8.Services.DBService;
using Realms;
using Realms.Exceptions;
using System.Diagnostics;
using System.Threading.Tasks;
using R = MauiApp8.Services.BackgroundServices.Realm;
using System.Linq;
//using Xamarin.Google.Crypto.Tink.Shaded.Protobuf;


namespace MauiApp8.Services.BackgroundServices
{
    public class DataBase : IBackgroundService
    {
      
        public static string Pathgetter()
        {
            string executablePath = Environment.ProcessPath;
            string directoryPath = Path.GetDirectoryName(executablePath);
            string projectDirectoryPath = Path.GetFullPath(Path.Combine(directoryPath, "..", "..", ".."));
            return projectDirectoryPath;
        }

        public static Realms.Realm RealmCreate()
        {
            string pathToDb = $"{AppDomain.CurrentDomain.BaseDirectory}";
            var config = new RealmConfiguration(pathToDb + "/my.realm")
            {
                IsReadOnly = false,
            };
            Realms.Realm localRealm;
            try
            {
                localRealm = Realms.Realm.GetInstance(config);
                return localRealm;
            }
            catch (RealmFileAccessErrorException ex)
            {
                Console.WriteLine($@"Error creating or opening the realm file. A! {ex.Message}");
                return null;
            }
        }
        public async Task<float?> ReadLatestGlucoseValue()
        {
            Realms.Realm realm = CreateDB.RealmCreate();
            var objects = realm.All<Realm.GlucoseInfo>();
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
        public DateTimeOffset ReadCurrentTime()
        {
            DateTimeOffset utcOffset = DateTimeOffset.UtcNow;
            return utcOffset;
        }

        public async Task AddGlucoseEntry(float sgv, DateTimeOffset date)
        {
            Realms.Realm localRealm = CreateDB.RealmCreate();

            
            try
            {
                //string dbFullPath = localRealm.Config.DatabasePath;
                var GlucoseEntry = new Realm.GlucoseInfo { Glucose = sgv, Timestamp = date };
                await localRealm.WriteAsync(() =>
                {
                    localRealm.Add(GlucoseEntry);
                });
               // Console.WriteLine($"Adding Glucose Entry :  Glucose: {GlucoseEntry.Glucose}, Timestamp: {GlucoseEntry.Timestamp} ");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                localRealm.Dispose();
            }
        }

        public async Task AddInsulinEntry(double? insulin, double? basal, DateTimeOffset date)
        {
            Realms.Realm localRealm = RealmCreate();

            try
            {
                string dbFullPath = localRealm.Config.DatabasePath;
                var InsulinEntry = new Realm.InsulinInfo { Insulin = (double)insulin,Basal = (double)basal, Timestamp = date };
                await localRealm.WriteAsync(() =>
                {
                    localRealm.Add(InsulinEntry);
                });
               // Console.WriteLine($"Adding Insulin Entry :  Insulin: {InsulinEntry.Insulin}, Basal: {InsulinEntry.Basal}, Timestamp: {InsulinEntry.Timestamp} ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                localRealm.Dispose();
            }
        }

        public DateTimeOffset? ReadLatestGlucose()
        {
            Realms.Realm localRealm = CreateDB.RealmCreate();
            var objects = localRealm.All<Realm.GlucoseInfo>();

            DateTimeOffset? maxDateTime = objects.OrderByDescending(item => item.Timestamp).FirstOrDefault()?.Timestamp;
            if (maxDateTime == null)
            {
                Console.WriteLine("The Glucose List is empty");
                DateTimeOffset utcOffset = DateTimeOffset.UtcNow;
                localRealm.Dispose();
                return utcOffset.AddDays(-60);
            }
            else
            {
                Console.WriteLine("The Glucose List is not empty");
                DateTimeOffset dateTimeOffset = maxDateTime?.DateTime ?? DateTimeOffset.MinValue;
                TimeZoneInfo norwayTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

                // Convert the UTC time to Norwegian time
                DateTimeOffset norwayTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.UtcDateTime, norwayTimeZone);
                localRealm.Dispose();
                return norwayTime;
            }

        }

        public DateTimeOffset? ReadLatestInsulin()
        {
            Realms.Realm localRealm = RealmCreate();
            var objects_With = localRealm.All<Realm.InsulinInfo>();
            var objects = objects_With.Where(g => g.Insulin != 0);

            DateTimeOffset? maxDateTime = objects.OrderByDescending(item => item.Timestamp).FirstOrDefault()?.Timestamp;
            if (maxDateTime ==  null)
            {
                Console.WriteLine("The insulin List is empty");
                DateTimeOffset utcOffset = DateTimeOffset.UtcNow;
                localRealm.Dispose();
                return utcOffset.AddDays(-7);
            }
            else
            {
                Console.WriteLine("The insulin List is not empty");
                DateTimeOffset dateTimeOffset = maxDateTime?.DateTime ?? DateTimeOffset.MinValue;
                TimeZoneInfo norwayTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

                // Convert the UTC time to Norwegian time
                DateTimeOffset norwayTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.UtcDateTime, norwayTimeZone);
                localRealm.Dispose();
                return norwayTime;
            }


        }

        public async Task<int> UpdateGlucose(string DomainName)
        {
            DataBase DB = new DataBase();
            

            DateTimeOffset? utcStart = DB.ReadLatestGlucose();
            if (utcStart.HasValue == false)
            {
                return -1;
            }

            DateTimeOffset utcStartPlus = ((DateTimeOffset)utcStart).AddMinutes(5);
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo norwegianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime utcEnd = TimeZoneInfo.ConvertTimeFromUtc(utcTime, norwegianTimeZone);

            List<GlucoseAPI> Items;
            Items = await Nightscout.GetGlucose(DomainName, utcStartPlus.ToString("yyyy-MM-ddTHH:mm:ss"), utcEnd.ToString("yyyy-MM-ddTHH:mm:ss"));
            Console.WriteLine("Glucose entries added: " + Items.Count.ToString());
            Realms.Realm localRealm = CreateDB.RealmCreate();
            List<Task> tasks = new List<Task>();
    

          
            
            foreach (GlucoseAPI obj in Items)
            {
                if (obj.sgv != 0)
                {

                    tasks.Add(Task.Run(async () =>
                    {

                        tasks.Add(DB.AddGlucoseEntry(obj.sgv, obj.dateString));
                       


                    }));

                }
            }
           
            
            await Task.WhenAll(tasks);

            return 200;
        }

        public async Task<int> UpdateInsulin(string DomainName)
        {
            DataBase DB = new DataBase();
           

            DateTimeOffset? utcStart = DB.ReadLatestInsulin();

            if (utcStart.HasValue == false)
            {
                return -1;
            }

            DateTimeOffset utcStartPlus = ((DateTimeOffset)utcStart).AddMinutes(5);
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo norwegianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime utcEnd = TimeZoneInfo.ConvertTimeFromUtc(utcTime, norwegianTimeZone);
            List<TreatmentAPI> Items;
            double? basal;
            Items = await Nightscout.GetInsulin(DomainName, utcStartPlus.ToString("yyyy-MM-ddTHH:mm:ss"), utcEnd.ToString("yyyy-MM-ddTHH:mm:ss"));
            Console.WriteLine("Insulin entries added: " + Items.Count.ToString());


            var stopwatch = new Stopwatch();
            stopwatch.Start();
           
           



            List<Task> tasks = new List<Task>();
            float start = GC.GetTotalMemory(true);
            foreach (TreatmentAPI obj in Items)
            {
                if (obj.insulin != null)
                {
                    TimeZoneInfo norwegianTime = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                    DateTimeOffset utcBasalTime = TimeZoneInfo.ConvertTimeFromUtc(obj.created_at, norwegianTime);
                    //basal = await GetBasalInsulin(DomainName, obj.created_at);
                    basal = 0;
                    tasks.Add(Task.Run(async () =>
                    {
                        
                             await DB.AddInsulinEntry((double)obj.insulin, (double)basal, obj.created_at);
                        
                      
                    }));

                }
            }

            //basal = await GetBasalInsulin(DomainName, utcEnd);
            basal = 0;
            Console.WriteLine("remember");
            tasks.Add(Task.Run(async () =>
            {
                
                    await DB.AddInsulinEntry((double)0, (double)basal, utcEnd);
                
                
            }));

           


            return 200;
        }

        public DateTimeOffset Get_NewestTimestamp(DateTimeOffset first_datetime, DateTimeOffset second_datetime)
        {       
           
           
            if (DateTimeOffset.Compare(first_datetime, second_datetime) < 0)
            {
                //Console.WriteLine("Newest: " + second_datetime);
                return second_datetime;
            }
            else
            {
                //Console.WriteLine("Newest: " + first_datetime);
                return first_datetime;
            }
        }

        public async Task<double?> GetBasalInsulin(string DomainName, DateTimeOffset time) {
            //2023-04-12T19:34:56.626Z
            Console.WriteLine("Before GetBasalInsulin");
            DateTimeOffset ClosestTime;
            DateTimeOffset ClosestValue;
            List<BasalData.NightscoutProfile> Items;
            Items = await Nightscout.GetInsulinBasal(DomainName);
            //Console.WriteLine(Items.Count);
            foreach (BasalData.NightscoutProfile obj in Items)
            {
                //Console.WriteLine("Created at: " + obj.CreatedAt);
                ClosestTime = Get_NewestTimestamp(time, obj.CreatedAt);
                if ( ClosestTime.ToString() == time.ToString()) { 
                    //Console.WriteLine(obj.CreatedAt.ToString());
                    foreach (var storeEntry in obj.Store)
                    {
                        var storeName = storeEntry.Key;
                        var storeData = storeEntry.Value;
                        DateTimeOffset newDateTime = obj.CreatedAt;
                        for (int i = 0; i < storeData.Basal.Count; i++)
                        {
                            var basal = storeData.Basal[i];
                            DateTimeOffset newDateTimeOffset = newDateTime.AddSeconds(Double.Parse(basal.TimeAsSeconds, System.Globalization.CultureInfo.InvariantCulture));
                            ClosestValue = Get_NewestTimestamp(newDateTimeOffset, time);
                            if(ClosestValue != time) { Console.WriteLine("Getting Basal..."); return Double.Parse(basal.Value, System.Globalization.CultureInfo.InvariantCulture); }
                            if(i == storeData.Basal.Count - 1) { Console.WriteLine("Getting Basal..."); return Double.Parse(basal.Value, System.Globalization.CultureInfo.InvariantCulture); }
                            newDateTime = newDateTimeOffset;
                        }
                    }
                    break; 
                }
                else if (obj.Equals(Items.Last()))
                {
                    //Console.WriteLine("This is the last object in Items!");
                    //Console.WriteLine(obj.CreatedAt.ToString());
                    foreach (var storeEntry in obj.Store)
                    {
                        var storeName = storeEntry.Key;
                        var storeData = storeEntry.Value;
                        DateTimeOffset newDateTime = obj.CreatedAt;
                        for (int i = 0; i < storeData.Basal.Count; i++)
                        {
                            var basal = storeData.Basal[i];
                            DateTimeOffset newDateTimeOffset = newDateTime.AddSeconds(Double.Parse(basal.TimeAsSeconds, System.Globalization.CultureInfo.InvariantCulture));
                            ClosestValue = Get_NewestTimestamp(newDateTimeOffset, time);
                            if (ClosestValue != time) { Console.WriteLine("Getting Basal..."); return Double.Parse(basal.Value, System.Globalization.CultureInfo.InvariantCulture); }
                            if (i == storeData.Basal.Count - 1) { Console.WriteLine("Getting Basal..."); return Double.Parse(basal.Value, System.Globalization.CultureInfo.InvariantCulture); }
                            newDateTime = newDateTimeOffset;

                        }
                    }
                    break;
                }
            }
            

            return 0;
        }


        public async void TestDBContentInput() //Configuration
        {

            Realms.Realm localRealm = RealmCreate();

            var numberOfIterations = 10;
            var i = 0;
            List<string> myList = new List<string>();
            myList.Add(","); myList.Add(".");  myList.Add("-"); myList.Add("_"); myList.Add("*"); myList.Add("`"); myList.Add("'"); myList.Add("^");
            myList.Add("¨"); myList.Add("\\"); myList.Add("+"); myList.Add("|"); myList.Add("<"); myList.Add(">"); myList.Add("1"); myList.Add("k");
            myList.Add("!"); myList.Add("\""); myList.Add("#"); myList.Add("¤"); myList.Add("&"); myList.Add("/"); myList.Add("("); myList.Add(")"); myList.Add("="); myList.Add("?");
            myList.Add("@"); myList.Add("£"); myList.Add("$"); myList.Add("€"); myList.Add("{"); myList.Add("["); myList.Add("]"); myList.Add("}"); myList.Add("æ"); myList.Add("ø");
            myList.Add("å"); myList.Add("Æ"); myList.Add("Ø"); myList.Add("Å");

            while (i < numberOfIterations)
            {
                Console.WriteLine(myList[i] + myList[i + 10] + myList[i + 20] + myList[i + 30]);
                using (var trans = localRealm.BeginWrite())
                {
                    var newObj = new R.Configuration
                    {
                        NightscoutAPI = myList[i],
                        NightscoutSecret = myList[i + 10],
                        HealthKitAPI = myList[i + 20],
                        HealthKitSecret = myList[i + 30],
                        GPU = true
                    };
                    localRealm.Add(newObj);
                    trans.Commit();
                }

                i++;
            }
            await Task.CompletedTask;

        }

        public async void TestDBAmountInput(int NumberToAdd) //Configuration
        {
            Console.WriteLine("Adding...");
            Realms.Realm localRealm = RealmCreate();
            var numberOfIterations = 200;
            var i = 0;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //float start = GC.GetTotalMemory(true);


            Console.WriteLine("Iterating...");
            while (i < NumberToAdd)
            {
                using (var trans = localRealm.BeginWrite())
                {
                    var newObj = new R.Configuration
                    {
                        NightscoutAPI = "oskar",
                        NightscoutSecret = "oskar",
                        HealthKitAPI = "oskar",
                        HealthKitSecret = "oskar",
                        GPU = true
                    };
                    localRealm.Add(newObj);
                    trans.Commit();
                }
                //Console.WriteLine(i);
                i++;
            }
            Console.WriteLine($"Finished Adding {NumberToAdd} entries!!");
            float end = GC.GetTotalMemory(true);
            //Console.WriteLine($"Total memory used: {end - start}");
            stopwatch.Stop();

            var elapsed = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Stopwatch time elapsed: {elapsed}");
            await Task.CompletedTask;
        }

        public async void TestDBAmountDEL() //Configuration
        {
            Console.WriteLine("Deleting...");
            Realms.Realm realm = RealmCreate();
            // Select the entries to be deleted
            var entriesToDelete = realm.All<R.Configuration>().Take(10);
            //realm.All<R.Configuration>().Where(o => o.NightscoutAPI == "oskar").Limit(10);
            //var entriesToDelete = realm.All<R.Configuration>().Take(323);

            // Delete the selected entries using a transaction
            //var entriesToDelete = realm.All<R.Configuration>().Where((r, index) => index < 323).ToList();
            //var entriesToDelete = realm.All<R.Configuration>();
            var stopwatch = new Stopwatch();
            stopwatch.Start();



            float start = GC.GetTotalMemory(true);

            int i = 0;
            Console.WriteLine($"Amount of entries that is going to be deleted: {entriesToDelete.Count()}");
            realm.Write(() =>
            {
                foreach (var entry in entriesToDelete)
                {
                    realm.Remove(entry);
                    i++;
                    if (i == 16452) { Console.WriteLine("BREAK"); break; } ;
                }
            });



            float end = GC.GetTotalMemory(true);
            Console.WriteLine($"Total memory used: {end - start}");
            stopwatch.Stop();

            var elapsed = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Stopwatch time elapsed: {elapsed}");
            await Task.CompletedTask;

        }
        

    }
}


