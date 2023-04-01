using Realms.Exceptions;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiApp8.Model2;
using System.Data.Common;

namespace MauiApp8.Services.BackgroundServices
{
    public class DataBase: IBackgroundService
    {


        public string bgl { get; set; }
        public string insulin { get; set; }

        private Realm _db;

        public DataBase(Realm relm)
        {
            _db = relm;
        }
        //public static string Pathgetter()
        //{
        //    string path = System.Reflection.Assembly.GetCallingAssembly().CodeBase;
        //    string actualPath = path.Substring(0, path.LastIndexOf("bin"));
        //    actualPath = actualPath.Substring(0, actualPath.LastIndexOf("/"));
        //    //actualPath = actualPath.Substring(0, actualPath.LastIndexOf("/"));
        //    string projectPath = new Uri(actualPath).LocalPath;
        //    path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, projectPath);
        //    return path;
        //}
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
            //Console.WriteLine(pathToDb + "/my.realm");
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
            try
            {
                string dbFullPath = _db.Config.DatabasePath;
                var GlucoseEntry = new GlucoseInfo { Glucose = sgv, Timestamp = date };
                await _db.WriteAsync((tmpRealm) =>
                {
                    tmpRealm.Add(GlucoseEntry);
                });

                _db.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async void AddInsulinEntry(double? insulin, DateTimeOffset date)
        {
            try
            {
                string dbFullPath = _db.Config.DatabasePath;
                var InsulinEntry = new InsulinInfo { Insulin = (double)insulin, Timestamp = date };
                await _db.WriteAsync((tmpRealm) =>
                {
                    tmpRealm.Add(InsulinEntry);
                });

                _db.Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public DateTimeOffset? ReadLatestGlucose()
        {
            var objects = _db.All<GlucoseInfo>();
            // Find the maximum DateTimeOffset value of the property
            DateTimeOffset? maxDateTime = objects.OrderByDescending(item => item.Timestamp).FirstOrDefault()?.Timestamp;
            if (maxDateTime == null)
            {
                Console.WriteLine("The Glucose List is empty");
                DateTimeOffset utcOffset = DateTimeOffset.UtcNow;
                return utcOffset.AddMonths(-1);
            }
            else
            {
                Console.WriteLine("The Glucose List is not empty");
                DateTimeOffset dateTimeOffset = maxDateTime?.DateTime ?? DateTimeOffset.MinValue;
                TimeZoneInfo norwayTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

                // Convert the UTC time to Norwegian time

                DateTimeOffset norwayTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.UtcDateTime, norwayTimeZone);
                return norwayTime;
            }

        }


        public DateTimeOffset? ReadLatestInsulin()
        {
            var objects = _db.All<InsulinInfo>();

            DateTimeOffset? maxDateTime = objects.OrderByDescending(item => item.Timestamp).FirstOrDefault()?.Timestamp;
            if (maxDateTime == null)
            {

                Console.WriteLine("The insulin List is empty");
                DateTimeOffset utcOffset = DateTimeOffset.UtcNow;
                _db.Dispose();
                return utcOffset.AddMonths(-1);
            }
            else
            {
                Console.WriteLine("The insulin List is not empty");
                DateTimeOffset dateTimeOffset = maxDateTime?.DateTime ?? DateTimeOffset.MinValue;
                TimeZoneInfo norwayTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

                // Convert the UTC time to Norwegian time

                DateTimeOffset norwayTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeOffset.UtcDateTime, norwayTimeZone);
                _db.Dispose();
                return norwayTime;
            }
        }

        public async Task<int> UpdateGlucose(string DomainName)
        {

            DateTimeOffset? utcStart =  ReadLatestGlucose();
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
                Console.WriteLine($"Loaded {obj.sgv} in updateglukose in BGService");

                AddGlucoseEntry(obj.sgv, obj.dateString);
            }
            return 200;
        }

        public async Task<int> UpdateInsulin(string DomainName)
        {

            DateTimeOffset? utcStart = ReadLatestInsulin();

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
            Items = await Nightscout.GetInsulin(DomainName, utcStartPlus.ToString("yyyy-MM-ddTHH:mm:ss"), utcEnd.ToString("yyyy-MM-ddTHH:mm:ss"));
            Console.WriteLine("Adding " + Items.Count + " entries... ");
            foreach (TreatmentAPI obj in Items)
            {
                if (obj.insulin != null)
                    Console.WriteLine($"Loaded {obj.insulin} in UpdateInsulin in BGService");
                AddInsulinEntry((double)obj.insulin, obj.created_at);
            }
            return 200;
        }

        
    }
}


