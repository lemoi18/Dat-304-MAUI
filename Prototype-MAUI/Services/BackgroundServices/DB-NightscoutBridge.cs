using Realms.Exceptions;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp8.Model2;
using System.Data.Common;
using System.Globalization;
using MauiApp8.Model;
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

        public static Realm RealmCreate()
        {
            string pathToDb = $"{AppDomain.CurrentDomain.BaseDirectory}";
            var config = new RealmConfiguration(pathToDb + "/my.realm")
            {
                IsReadOnly = false,
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

        public DateTimeOffset ReadCurrentTime()
        {
            DateTimeOffset utcOffset = DateTimeOffset.UtcNow;
            return utcOffset;
        }

        public async void AddGlucoseEntry(float sgv, DateTimeOffset date)
        {
            Realm localRealm = RealmCreate();

            try
            {
                string dbFullPath = localRealm.Config.DatabasePath;
                var GlucoseEntry = new GlucoseInfo { Glucose = sgv, Timestamp = date };
                await localRealm.WriteAsync((tmpRealm) =>
                {
                    tmpRealm.Add(GlucoseEntry);
                });

                localRealm.Refresh();
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

        public async void AddInsulinEntry(double? insulin, DateTimeOffset date)
        {
            Realm localRealm = RealmCreate();

            try
            {
                string dbFullPath = localRealm.Config.DatabasePath;
                var InsulinEntry = new InsulinInfo { Insulin = (double)insulin, Timestamp = date };
                await localRealm.WriteAsync((tmpRealm) =>
                {
                    tmpRealm.Add(InsulinEntry);
                });

                localRealm.Refresh();
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
            Realm localRealm = RealmCreate();
            var objects = localRealm.All<GlucoseInfo>();
            // Find the maximum DateTimeOffset value of the property
            DateTimeOffset? maxDateTime = objects.OrderByDescending(item => item.Timestamp).FirstOrDefault()?.Timestamp;
            if (maxDateTime == null)
            {
                Console.WriteLine("The Glucose List is empty");
                DateTimeOffset utcOffset = DateTimeOffset.UtcNow;
                localRealm.Dispose();
                return utcOffset.AddMonths(-1);
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
            Realm localRealm = RealmCreate();
            var objects = localRealm.All<InsulinInfo>();

            DateTimeOffset? maxDateTime = objects.OrderByDescending(item => item.Timestamp).FirstOrDefault()?.Timestamp;
            if (maxDateTime == null)
            {
                Console.WriteLine("The insulin List is empty");
                DateTimeOffset utcOffset = DateTimeOffset.UtcNow;
                localRealm.Dispose();
                return utcOffset.AddMonths(-1);
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

        public async  Task<int> UpdateGlucose(string DomainName)
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
            Console.WriteLine("Adding " + Items.Count + " entries... ");
            foreach (GlucoseAPI obj in Items)
            {
                if (obj.sgv != 0)
                    DB.AddGlucoseEntry(obj.sgv, obj.dateString);
            }
            return 200;
        }

        public async  Task<int> UpdateInsulin(string DomainName)
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
            Items = await Nightscout.GetInsulin(DomainName, utcStartPlus.ToString("yyyy-MM-ddTHH:mm:ss"), utcEnd.ToString("yyyy-MM-ddTHH:mm:ss"));
            Console.WriteLine("Adding " + Items.Count + " entries... ");
            foreach (TreatmentAPI obj in Items)
            {
                if (obj.insulin != null)
                {
                    Console.WriteLine(obj.insulin);
                    TimeZoneInfo norwegianTime = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                    DateTimeOffset utcBasalTime = TimeZoneInfo.ConvertTimeFromUtc(obj.created_at, norwegianTime);
                    DB.AddInsulinEntry((double)obj.insulin, obj.created_at);

                }
            }
            return 200;
        }

        public DateTimeOffset Get_NewestTimestamp(DateTimeOffset first_datetime, DateTimeOffset second_datetime)
        {       
           
            Console.WriteLine(first_datetime);
            Console.WriteLine(second_datetime);
            if (DateTimeOffset.Compare(first_datetime, second_datetime) < 0)
            {
                Console.WriteLine("Newest: " + second_datetime);
                return second_datetime;
            }
            else
            {
                Console.WriteLine("Newest: " + first_datetime);
                return first_datetime;
            }
        }

        public async Task<double?> GetBasalInsulin(string DomainName, DateTimeOffset time) {
            //2023-04-12T19:34:56.626Z
            DateTimeOffset ClosestTime;
            DateTimeOffset ClosestValue;
            List<BasalData.NightscoutProfile> Items;
            Items = await Nightscout.GetInsulinBasal(DomainName);
            Console.WriteLine(Items.Count);
            foreach (BasalData.NightscoutProfile obj in Items)
            {
                Console.WriteLine("Created at: " + obj.CreatedAt);
                ClosestTime = Get_NewestTimestamp(time, obj.CreatedAt);
                if ( ClosestTime.ToString() == time.ToString()) { 
                    Console.WriteLine(obj.CreatedAt.ToString());
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
                    Console.WriteLine("This is the last object in Items!");
                    Console.WriteLine(obj.CreatedAt.ToString());
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

    }
}


