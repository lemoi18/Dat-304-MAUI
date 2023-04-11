using Realms;
using Realms.Exceptions;

namespace MauiApp8.Services.BackgroundServices.Realm
{
    internal class Utils
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

        public static void RealmDelete()
        {
            string realmPath = RealmConfiguration.DefaultConfiguration.DatabasePath;

            if (File.Exists(realmPath))
            {
                Console.WriteLine("Deleting realm file");
                File.Delete(realmPath);
                
            }
        }
    }
}
