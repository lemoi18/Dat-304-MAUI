using Realms;
using static DAT304_MAUI.Backend.Realm.Utils;
using static DAT304_MAUI.Backend.Nightscout;
using DAT304_MAUI.Backend.Realm.Schemas;


namespace DAT304_MAUI.Backend.Realm
{
    internal class CRUD
    {
        public async void AddGlucoseEntry(float sgv, DateTimeOffset date)
        {
            Realms.Realm localRealm = RealmCreate();

            try
            {
                string dbFullPath = localRealm.Config.DatabasePath;
                var GlucoseEntry = new GlucoseInfo { Glucose = sgv, Timestamp = date };
                await localRealm.WriteAsync(() =>
                {
                    localRealm.Add(GlucoseEntry);
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
            Realms.Realm localRealm = RealmCreate();

            try
            {
                string dbFullPath = localRealm.Config.DatabasePath;
                var InsulinEntry = new InsulinInfo { Insulin = (double)insulin, Timestamp = date };
                await localRealm.WriteAsync(() => {
                    localRealm.Add(InsulinEntry);
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
            Realms.Realm localRealm = RealmCreate();
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
            Realms.Realm localRealm = RealmCreate();
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

        public async static Task<int> UpdateGlucose(string DomainName)
        {
            CRUD DB = new CRUD();

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
            Items = await GetGlucose(DomainName, utcStartPlus.ToString("yyyy-MM-ddTHH:mm:ss"), utcEnd.ToString("yyyy-MM-ddTHH:mm:ss"));
            Console.WriteLine("Adding " + Items.Count + " entries... ");
            foreach (GlucoseAPI obj in Items)
            {
                DB.AddGlucoseEntry(obj.sgv, obj.dateString);
            }
            return 200;
        }

        public async static Task<int> UpdateInsulin(string DomainName)
        {
            CRUD DB = new CRUD();

            DateTimeOffset? utcStart = DB.ReadLatestInsulin();

            if (utcStart.HasValue == false)
            {
                return -1;
            }

            DateTimeOffset utcStartPlus = ((DateTimeOffset)utcStart).AddMinutes(5);
            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo norwegianTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime utcEnd = TimeZoneInfo.ConvertTimeFromUtc(utcTime, norwegianTimeZone);
            Console.WriteLine(utcStartPlus.ToString("yyyy-MM-ddTHH:mm:ss"));
            Console.WriteLine(utcEnd.ToString("yyyy-MM-ddTHH:mm:ss"));
            List<TreatmentAPI> Items;
            Items = await GetInsulin(DomainName, utcStartPlus.ToString("yyyy-MM-ddTHH:mm:ss"), utcEnd.ToString("yyyy-MM-ddTHH:mm:ss"));
            Console.WriteLine("Adding " + Items.Count + " entries... ");
            foreach (TreatmentAPI obj in Items)
            {
                if (obj.insulin != null)
                    DB.AddInsulinEntry((double)obj.insulin, obj.created_at);
            }
            return 200;
        }

    }
}
