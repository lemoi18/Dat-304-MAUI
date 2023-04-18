using CommunityToolkit.Mvvm.ComponentModel;
using MauiApp8.Services.BackgroundServices.Realm;

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
