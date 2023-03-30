using Realms;
using Realms.Exceptions;
namespace CreateDBLib
{
    public class CreateDB
    {

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

        
        
    }
}