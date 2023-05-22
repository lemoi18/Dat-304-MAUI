using MauiApp8.Model;

namespace MauiApp8.Services.BackgroundServices
{
    public class BackgroundServiceStub : IBackgroundService
    {
        private Realms.Realm _db;

        public BackgroundServiceStub(Realms.Realm relm)
        {
            _db = relm;
            Console.WriteLine("Test from BGService");
        }

        private readonly List<GlucoseInfo> _glucoseEntries = new List<GlucoseInfo>();
        private readonly List<InsulinInfo> _insulinEntries = new List<InsulinInfo>();

     
        

        public DateTimeOffset? ReadLatestGlucose()
        {
            return _glucoseEntries.OrderByDescending(item => item.Timestamp).FirstOrDefault()?.Timestamp;
        }

        public DateTimeOffset? ReadLatestInsulin()
        {
            return _insulinEntries.OrderByDescending(item => item.Timestamp).FirstOrDefault()?.Timestamp;
        }

        public Task<int> UpdateGlucose(string domainName)
        {
            return Task.FromResult(0);
        }

        public Task<int> UpdateInsulin(string domainName)
        {
            return Task.FromResult(0);
        }

        public DateTimeOffset Get_NewestTimestamp(DateTimeOffset first_datetime, DateTimeOffset second_datetime) { DateTimeOffset r = new DateTimeOffset(); return r; }

        public async Task<double?> GetBasalInsulin(string DomainName, DateTimeOffset time) { return 22; }

      

        public Task AddInsulinEntry(double? insulin, double? basal, DateTimeOffset date)
        {
            throw new NotImplementedException();
        }

        public Task<float?> ReadLatestGlucoseValue()
        {
            throw new NotImplementedException();
        }

        public Task AddGlucoseEntries(List<Realm.GlucoseInfo> entries)
        {
            throw new NotImplementedException();
        }

        public Task AddInsulinEntries(List<Realm.InsulinInfo> entries)
        {
            throw new NotImplementedException();
        }
    }
}


