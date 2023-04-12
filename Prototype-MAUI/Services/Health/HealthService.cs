using MauiApp8.Services.BackgroundServices.Realm;

namespace MauiApp8.Services.Health
{
    internal class HealthService : IHealthService
    {
        private readonly IUtils _utils;
        private readonly ICRUD _crud;

        public HealthService(IUtils utils, ICRUD crud)
        {
            _utils = utils;
            _crud = crud;
        }

        public List<Model.GlucoseInfo> ReadGlucoses(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            using var realm = _utils.RealmCreate();
            return _crud.ReadGlucoses(realm, fromDate, toDate);
        }

        public List<Model.InsulinInfo> ReadInsulins(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            using var realm = _utils.RealmCreate();
            return _crud.ReadInsulins(realm, fromDate, toDate);
        }
    }
}
