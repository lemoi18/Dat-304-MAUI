namespace MauiApp8.Services.BackgroundServices.Realm
{
    public interface IUtils
    {
        string Pathgetter();
        Realms.Realm RealmCreate();
        void RealmDelete();
    }
}
