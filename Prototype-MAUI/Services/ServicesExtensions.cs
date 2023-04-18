using MauiApp8.Services.GraphService;
using MauiApp8.Services.Health;
using MauiApp8.Services.Authentication;
using MauiApp8.Services.BackgroundServices.Realm;
using MauiApp8.Model;
using MauiApp8.Services.Food;

namespace MauiApp8.Services
{
    public static class ServicesExtensions
    {
        public static MauiAppBuilder ConfigureServices(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IChartService<HealthData>>((e) => new LineChartService<HealthData>(e.GetRequiredService<PublishSubscribeService.Publish>(), e.GetRequiredService<IHealthService>()));
            builder.Services.AddSingleton<IChartConfigurationProvider>((e) => new ChartConfigurationProvider(e.GetRequiredService<IChartService<HealthData>>()));
            builder.Services.AddTransient<BackgroundServices.Realm.ICRUD>((e) => new Services.BackgroundServices.Realm.CRUD());
            builder.Services.AddTransient<BackgroundServices.Realm.IUtils>((e) => new Services.BackgroundServices.Realm.Utils());
            builder.Services.AddTransient<IHealthService>((e) => new HealthService(e.GetService<IUtils>(), e.GetService<ICRUD>()));
            builder.Services.AddSingleton<Authentication.IAuthenticationService>((e) => new Services.Authentication.Authenticated_stub());
            builder.Services.AddTransient<Realms.Realm>(e => Services.DBService.CreateDB.RealmCreate());
            builder.Services.AddTransient<Services.BackgroundServices.IBackgroundService>((e) => new Services.BackgroundServices.DataBase());
            builder.Services.AddSingleton<Services.ThirdPartyHealthService.IThirdPartyHealthService>(e => new Services.ThirdPartyHealthService.GoogleFit(e.GetRequiredService<IAuthenticationService>()));
            builder.Services.AddSingleton<Services.PublishSubscribeService.Publish>(e => new Services.PublishSubscribeService.Publish(e.GetRequiredService<Services.BackgroundServices.IBackgroundService>(), e.GetRequiredService<Services.ThirdPartyHealthService.IThirdPartyHealthService>(), e.GetRequiredService<Services.Health.IHealthService>()));
#if __ANDROID__
            builder.Services.AddSingleton<Services.BackgroundFetchService.IBackgroundFetchService, MauiApp8.Platforms.Android.AndroidServices.BackgroundFetchServiceAndroid>(services => new MauiApp8.Platforms.Android.AndroidServices.BackgroundFetchServiceAndroid());
#endif
            builder.Services.AddSingleton<IFoodService, FoodService>();
            return builder;
        }
    }
}
