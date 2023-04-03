using DAT304_MAUI.Backend.Realm;
using System;

namespace DAT304_MAUI.Tests
{
    public class CRUDTests
    {
        private static CRUD _crud = new CRUD();

        public static async void RunTests()
        {
            Console.WriteLine("Running CRUD tests...");

            // Add your test cases here
            TestAddGlucoseEntry();
            TestAddInsulinEntry();
            TestReadLatestGlucose();
            TestReadLatestInsulin();

            await TestUpdateGlucose("https://oskarnightscoutweb1.azurewebsites.net/");
            await TestUpdateInsulin("https://oskarnightscoutweb1.azurewebsites.net/");

            //Console.WriteLine("CRUD tests completed.");
        }

        private static void TestAddGlucoseEntry()
        {
            float sgv = 150;
            DateTimeOffset date = DateTimeOffset.UtcNow;

            _crud.AddGlucoseEntry(sgv, date);
            Console.WriteLine("TestAddGlucoseEntry: Passed");
        }

        private static void TestAddInsulinEntry()
        {
            double? insulin = 5.0;
            DateTimeOffset date = DateTimeOffset.UtcNow;

            _crud.AddInsulinEntry(insulin, date);
            Console.WriteLine("TestAddInsulinEntry: Passed");
        }

        private static void TestReadLatestGlucose()
        {
            DateTimeOffset? latestGlucose = _crud.ReadLatestGlucose();
            Console.WriteLine($"TestReadLatestGlucose: Latest glucose date: {latestGlucose}");
        }

        private static void TestReadLatestInsulin()
        {
            DateTimeOffset? latestInsulin = _crud.ReadLatestInsulin();
            Console.WriteLine($"TestReadLatestInsulin: Latest insulin date: {latestInsulin}");
        }

        private static async System.Threading.Tasks.Task TestUpdateGlucose(string domainName)
        {
            int result = await CRUD.UpdateGlucose(domainName);
            Console.WriteLine($"TestUpdateGlucose: Result {result}");
        }

        private static async System.Threading.Tasks.Task TestUpdateInsulin(string domainName)
        {
            int result = await CRUD.UpdateInsulin(domainName);
            Console.WriteLine($"TestUpdateInsulin: Result {result}");
        }
    }
}
