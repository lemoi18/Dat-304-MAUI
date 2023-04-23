using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Services.BackgroundServices.Realm;
using Realms;

namespace MauiApp8.Services.Health
{
    public partial class HealthService : ObservableObject, IHealthService
    {
        [ObservableProperty]
         IUtils utilss;
        private readonly ICRUD _crud;



        public HealthService(IUtils utils, ICRUD crud)
        {
            Utilss = utils;
            _crud = crud;
        }

        public async Task<string> DeleteGlucoseData(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            var realm = Utilss.RealmCreate();

            var allObjects = realm.All<GlucoseInfo>().Where(g => g.Timestamp >= fromDate && g.Timestamp <= toDate);

            using (var transaction = realm.BeginWrite())
            {
                foreach (var obj in allObjects)
                {
                    realm.Remove(obj);
                }
                await transaction.CommitAsync();
            }

            return "Deleted glucose data";
        }

        public async Task<string> DeleteInsulinData(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            var realm = Utilss.RealmCreate();

            var allObjects = realm.All<InsulinInfo>().Where(i => i.Timestamp >= fromDate && i.Timestamp <= toDate);

            using (var transaction = realm.BeginWrite())
            {
                foreach (var obj in allObjects)
                {
                    realm.Remove(obj);
                }
                await transaction.CommitAsync();
            }

            return "Deleted insulin data";
        }



        public Task<List<Model.GlucoseInfo>> ReadGlucoses(DateTimeOffset fromDate, DateTimeOffset toDate)
        {

            using var realm = Utilss.RealmCreate();

            return Task.FromResult(_crud.ReadGlucoses(realm, fromDate, toDate).ConvertAll(ClassConvert.ToModel));
        }

        public Task<List<Model.InsulinInfo>> ReadInsulins(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            using var realm = Utilss.RealmCreate();

            return Task.FromResult(_crud.ReadInsulins(realm, fromDate, toDate).ConvertAll(ClassConvert.ToModel));
        }
    }
}
